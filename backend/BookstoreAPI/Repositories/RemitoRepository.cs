using BookstoreAPI.Data;
using BookstoreAPI.Models;
using Dapper;

namespace BookstoreAPI.Repositories
{
    public class RemitoRepository : IRemitoRepository
    {
        private readonly DapperContext _context;

        public RemitoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Remito>> GetAllAsync()
        {
            const string query = @"
                SELECT
                    r.id AS Id,
                    r.numero AS Numero,
                    r.fecha AS Fecha,
                    r.cliente_id AS ClienteId,
                    r.transporte_id AS TransporteId,
                    r.cantidad_bultos AS CantidadBultos,
                    r.valor_declarado AS ValorDeclarado,
                    r.observaciones AS Observaciones,
                    c.Nombre AS ClienteNombre,
                    c.DomicilioComercial AS ClienteDomicilio,
                    s.localidad AS ClienteLocalidad,
                    p.descripcion AS ClienteProvincia,
                    c.NroDocumento AS ClienteCuit,
                    c.CodigoPostal AS ClienteCodigoPostal,
                    t.nombre AS TransporteNombre,
                    t.direccion AS TransporteDireccion,
                    t.localidad AS TransporteLocalidad,
                    t.cuit AS TransporteCuit
                FROM remitos r
                INNER JOIN clientes c ON r.cliente_id = c.Id
                LEFT JOIN subzonas s ON c.SubZona_Id = s.id
                LEFT JOIN provincias p ON s.provincia_id = p.id
                INNER JOIN transportes t ON r.transporte_id = t.id
                ORDER BY r.fecha DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Remito>(query);
        }

        public async Task<Remito?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT
                    r.id AS Id,
                    r.numero AS Numero,
                    r.fecha AS Fecha,
                    r.cliente_id AS ClienteId,
                    r.transporte_id AS TransporteId,
                    r.cantidad_bultos AS CantidadBultos,
                    r.valor_declarado AS ValorDeclarado,
                    r.observaciones AS Observaciones,
                    c.Nombre AS ClienteNombre,
                    c.DomicilioComercial AS ClienteDomicilio,
                    s.localidad AS ClienteLocalidad,
                    p.descripcion AS ClienteProvincia,
                    c.NroDocumento AS ClienteCuit,
                    c.CodigoPostal AS ClienteCodigoPostal,
                    t.nombre AS TransporteNombre,
                    t.direccion AS TransporteDireccion,
                    t.localidad AS TransporteLocalidad,
                    t.cuit AS TransporteCuit
                FROM remitos r
                INNER JOIN clientes c ON r.cliente_id = c.Id
                LEFT JOIN subzonas s ON c.SubZona_Id = s.id
                LEFT JOIN provincias p ON s.provincia_id = p.id
                INNER JOIN transportes t ON r.transporte_id = t.id
                WHERE r.id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Remito>(query, new { Id = id });
        }

        public async Task<Remito> CreateAsync(Remito remito)
        {
            const string query = @"
                INSERT INTO remitos (numero, fecha, cliente_id, transporte_id, cantidad_bultos, valor_declarado, observaciones)
                VALUES (@Numero, @Fecha, @ClienteId, @TransporteId, @CantidadBultos, @ValorDeclarado, @Observaciones);
                SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, remito);
            remito.Id = id;
            return remito;
        }

        public async Task<Remito?> UpdateAsync(int id, Remito remito)
        {
            const string query = @"
                UPDATE remitos
                SET cliente_id = @ClienteId,
                    transporte_id = @TransporteId,
                    cantidad_bultos = @CantidadBultos,
                    valor_declarado = @ValorDeclarado,
                    observaciones = @Observaciones
                WHERE id = @Id";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new
            {
                Id = id,
                remito.ClienteId,
                remito.TransporteId,
                remito.CantidadBultos,
                remito.ValorDeclarado,
                remito.Observaciones
            });

            if (affectedRows == 0) return null;

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "DELETE FROM remitos WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<string> GetNextNumeroAsync()
        {
            // Formato: R-YYYYMMDD-XXXX
            var today = DateTime.Now;
            var datePrefix = $"R-{today:yyyyMMdd}-";

            const string query = @"
                SELECT numero FROM remitos
                WHERE numero LIKE @Prefix
                ORDER BY numero DESC
                LIMIT 1";

            using var connection = _context.CreateConnection();
            var lastNumero = await connection.QueryFirstOrDefaultAsync<string>(query, new { Prefix = datePrefix + "%" });

            int nextSequence = 1;
            if (!string.IsNullOrEmpty(lastNumero))
            {
                var lastSequence = lastNumero.Substring(lastNumero.LastIndexOf('-') + 1);
                if (int.TryParse(lastSequence, out int seq))
                {
                    nextSequence = seq + 1;
                }
            }

            return $"{datePrefix}{nextSequence:D4}";
        }
    }
}
