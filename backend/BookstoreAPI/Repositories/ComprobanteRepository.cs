using BookstoreAPI.Data;
using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using Dapper;
using System.Data;

namespace BookstoreAPI.Repositories
{
    public class ComprobanteRepository : IComprobanteRepository
    {
        private readonly DapperContext _context;
        private readonly ICuotaRepository _cuotaRepository;

        public ComprobanteRepository(DapperContext context, ICuotaRepository cuotaRepository)
        {
            _context = context;
            _cuotaRepository = cuotaRepository;
        }

        public async Task<IEnumerable<ComprobanteConDetallesDto>> GetAllAsync()
        {
            const string query = @"
                SELECT
                    c.id AS Id,
                    c.cliente_id AS Cliente_Id,
                    cl.Nombre AS ClienteNombre,
                    c.fecha AS Fecha,
                    c.tipoComprobante AS TipoComprobante,
                    c.numeroComprobante AS NumeroComprobante,
                    c.total AS Total,
                    c.CAE,
                    c.VTO,
                    c.Bonificacion,
                    c.PorcentajeBonif,
                    c.Anticipo,
                    c.Cuotas,
                    c.ValorCuota,
                    c.vendedor_id AS Vendedor_Id,
                    v.descripcion AS VendedorNombre
                FROM comprobantes c
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                LEFT JOIN vendedores v ON c.vendedor_id = v.id
                ORDER BY c.fecha DESC, c.id DESC";

            using var connection = _context.CreateConnection();
            var comprobantes = await connection.QueryAsync<ComprobanteConDetallesDto>(query);

            // Cargar detalles para cada comprobante
            foreach (var comprobante in comprobantes)
            {
                comprobante.Detalles = (await GetDetallesByComprobanteIdAsync(comprobante.Id, connection)).ToList();
            }

            return comprobantes;
        }

        public async Task<ComprobanteConDetallesDto?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT
                    c.id AS Id,
                    c.cliente_id AS Cliente_Id,
                    cl.Nombre AS ClienteNombre,
                    c.fecha AS Fecha,
                    c.tipoComprobante AS TipoComprobante,
                    c.numeroComprobante AS NumeroComprobante,
                    c.total AS Total,
                    c.CAE,
                    c.VTO,
                    c.Bonificacion,
                    c.PorcentajeBonif,
                    c.Anticipo,
                    c.Cuotas,
                    c.ValorCuota,
                    c.vendedor_id AS Vendedor_Id,
                    v.descripcion AS VendedorNombre
                FROM comprobantes c
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                LEFT JOIN vendedores v ON c.vendedor_id = v.id
                WHERE c.id = @Id";

            using var connection = _context.CreateConnection();
            var comprobante = await connection.QueryFirstOrDefaultAsync<ComprobanteConDetallesDto>(query, new { Id = id });

            if (comprobante != null)
            {
                comprobante.Detalles = (await GetDetallesByComprobanteIdAsync(id, connection)).ToList();
            }

            return comprobante;
        }

        public async Task<Comprobante?> GetComprobanteByIdAsync(int id)
        {
            const string query = @"
                SELECT
                    id AS Id,
                    cliente_id AS Cliente_Id,
                    fecha AS Fecha,
                    tipoComprobante AS TipoComprobante,
                    numeroComprobante AS NumeroComprobante,
                    total AS Total,
                    CAE,
                    VTO,
                    Bonificacion,
                    PorcentajeBonif,
                    Anticipo,
                    Cuotas,
                    ValorCuota,
                    vendedor_id AS Vendedor_Id
                FROM comprobantes
                WHERE id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Comprobante>(query, new { Id = id });
        }

        private async Task<IEnumerable<ComprobanteDetalleConArticuloDto>> GetDetallesByComprobanteIdAsync(int comprobanteId, IDbConnection connection)
        {
            const string query = @"
                SELECT
                    cd.id AS Id,
                    cd.articulo_id AS Articulo_Id,
                    a.Codigo AS ArticuloCodigo,
                    a.Descripcion AS ArticuloDescripcion,
                    cd.cantidad AS Cantidad,
                    cd.precio_unitario AS Precio_Unitario,
                    cd.subtotal AS Subtotal
                FROM comprobante_detalle cd
                INNER JOIN articulos a ON cd.articulo_id = a.Id
                WHERE cd.factura_id = @ComprobanteId
                ORDER BY cd.id";

            return await connection.QueryAsync<ComprobanteDetalleConArticuloDto>(query, new { ComprobanteId = comprobanteId });
        }

        public async Task<Comprobante> CreateAsync(Comprobante comprobante, List<ComprobanteDetalle> detalles)
        {
            const string comprobanteQuery = @"
                INSERT INTO comprobantes
                (cliente_id, fecha, tipoComprobante, numeroComprobante, total, CAE, VTO,
                 Bonificacion, PorcentajeBonif, Anticipo, Cuotas, ValorCuota, vendedor_id)
                VALUES
                (@Cliente_Id, @Fecha, @TipoComprobante, @NumeroComprobante, @Total, @CAE, @VTO,
                 @Bonificacion, @PorcentajeBonif, @Anticipo, @Cuotas, @ValorCuota, @Vendedor_Id);
                SELECT LAST_INSERT_ID();";

            const string detalleQuery = @"
                INSERT INTO comprobante_detalle
                (factura_id, articulo_id, cantidad, precio_unitario, subtotal)
                VALUES
                (@Factura_Id, @Articulo_Id, @Cantidad, @Precio_Unitario, @Subtotal)";

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var id = await connection.ExecuteScalarAsync<int>(comprobanteQuery, comprobante, transaction);
                comprobante.Id = id;

                foreach (var detalle in detalles)
                {
                    detalle.Factura_Id = id;
                    await connection.ExecuteAsync(detalleQuery, detalle, transaction);
                }

                // Guardar cuotas si anticipo > 0 (dentro de la transacción)
                if (comprobante.Anticipo > 0 && comprobante.Cuotas > 0 && comprobante.ValorCuota > 0)
                {
                    var cuotas = GenerarCuotas(comprobante);
                    await _cuotaRepository.CreateCuotasAsync(id, cuotas, connection, transaction);
                }

                transaction.Commit();
                return comprobante;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private List<Cuota> GenerarCuotas(Comprobante comprobante)
        {
            var cuotas = new List<Cuota>();
            var fechaBase = comprobante.Fecha;

            for (int i = 1; i <= comprobante.Cuotas; i++)
            {
                var cuota = new Cuota
                {
                    Comprobante_Id = comprobante.Id,
                    Fecha = fechaBase.AddMonths(i),
                    Importe = comprobante.ValorCuota,
                    Estado = "PEN"
                };
                cuotas.Add(cuota);
            }

            return cuotas;
        }

        public async Task<Comprobante?> UpdateAsync(int id, Comprobante comprobante, List<ComprobanteDetalle> detalles)
        {
            const string comprobanteQuery = @"
                UPDATE comprobantes
                SET cliente_id = @Cliente_Id,
                    fecha = @Fecha,
                    tipoComprobante = @TipoComprobante,
                    numeroComprobante = @NumeroComprobante,
                    total = @Total,
                    CAE = @CAE,
                    VTO = @VTO,
                    Bonificacion = @Bonificacion,
                    PorcentajeBonif = @PorcentajeBonif,
                    Anticipo = @Anticipo,
                    Cuotas = @Cuotas,
                    ValorCuota = @ValorCuota,
                    vendedor_id = @Vendedor_Id
                WHERE id = @Id";

            const string deleteDetallesQuery = "DELETE FROM comprobante_detalle WHERE factura_id = @Id";

            const string detalleQuery = @"
                INSERT INTO comprobante_detalle
                (factura_id, articulo_id, cantidad, precio_unitario, subtotal)
                VALUES
                (@Factura_Id, @Articulo_Id, @Cantidad, @Precio_Unitario, @Subtotal)";

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                var affectedRows = await connection.ExecuteAsync(comprobanteQuery,
                    new
                    {
                        Id = id,
                        comprobante.Cliente_Id,
                        comprobante.Fecha,
                        comprobante.TipoComprobante,
                        comprobante.NumeroComprobante,
                        comprobante.Total,
                        comprobante.CAE,
                        comprobante.VTO,
                        comprobante.Bonificacion,
                        comprobante.PorcentajeBonif,
                        comprobante.Anticipo,
                        comprobante.Cuotas,
                        comprobante.ValorCuota,
                        comprobante.Vendedor_Id
                    }, transaction);

                if (affectedRows == 0)
                {
                    transaction.Rollback();
                    return null;
                }

                // Eliminar detalles existentes
                await connection.ExecuteAsync(deleteDetallesQuery, new { Id = id }, transaction);

                // Insertar nuevos detalles
                foreach (var detalle in detalles)
                {
                    detalle.Factura_Id = id;
                    await connection.ExecuteAsync(detalleQuery, detalle, transaction);
                }

                // Actualizar cuotas (dentro de la transacción)
                await _cuotaRepository.DeleteByComprobanteIdAsync(id, connection, transaction);
                if (comprobante.Anticipo > 0 && comprobante.Cuotas > 0 && comprobante.ValorCuota > 0)
                {
                    comprobante.Id = id;
                    var cuotas = GenerarCuotas(comprobante);
                    await _cuotaRepository.CreateCuotasAsync(id, cuotas, connection, transaction);
                }

                transaction.Commit();
                comprobante.Id = id;
                return comprobante;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string deleteCuotasQuery = "DELETE FROM cuotas WHERE comprobante_id = @Id";
            const string deleteDetallesQuery = "DELETE FROM comprobante_detalle WHERE factura_id = @Id";
            const string deleteComprobanteQuery = "DELETE FROM comprobantes WHERE id = @Id";

            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(deleteCuotasQuery, new { Id = id }, transaction);
                await connection.ExecuteAsync(deleteDetallesQuery, new { Id = id }, transaction);
                var affectedRows = await connection.ExecuteAsync(deleteComprobanteQuery, new { Id = id }, transaction);

                transaction.Commit();
                return affectedRows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<ComprobanteDetalle>> GetDetallesByComprobanteIdAsync(int comprobanteId)
        {
            const string query = @"
                SELECT
                    id AS Id,
                    factura_id AS Factura_Id,
                    articulo_id AS Articulo_Id,
                    cantidad AS Cantidad,
                    precio_unitario AS Precio_Unitario,
                    subtotal AS Subtotal
                FROM comprobante_detalle
                WHERE factura_id = @ComprobanteId
                ORDER BY id";

            using var connection = _context.CreateConnection();
            var detalles = await connection.QueryAsync<ComprobanteDetalle>(query, new { ComprobanteId = comprobanteId });
            return detalles.ToList();
        }

        public async Task<IEnumerable<IvaVentasDto>> GetIvaVentasAsync(DateTime fechaDesde, DateTime fechaHasta)
        {
            const string query = @"
                SELECT
                    c.fecha AS Fecha,
                    c.numeroComprobante AS NumeroComprobante,
                    cl.Nombre AS Nombre,
                    cl.NroDocumento AS NroDocumento,
                    c.total AS Total
                FROM comprobantes c
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                WHERE c.fecha >= @FechaDesde AND c.fecha <= @FechaHasta
                ORDER BY c.fecha, c.numeroComprobante";

            using var connection = _context.CreateConnection();
            var ventas = await connection.QueryAsync<IvaVentasDto>(query, new { FechaDesde = fechaDesde, FechaHasta = fechaHasta });
            return ventas;
        }
    }
}
