using BookstoreAPI.Data;
using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using Dapper;
using System.Data;

namespace BookstoreAPI.Repositories
{
    public class CuotaRepository : ICuotaRepository
    {
        private readonly DapperContext _context;

        public CuotaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cuota>> GetByComprobanteIdAsync(int comprobanteId)
        {
            const string query = @"
                SELECT
                    id AS Id,
                    comprobante_id AS Comprobante_Id,
                    fecha AS Fecha,
                    importe AS Importe,
                    importe_pagado AS ImportePagado,
                    estado AS Estado
                FROM cuotas
                WHERE comprobante_id = @ComprobanteId
                ORDER BY fecha";

            using var connection = _context.CreateConnection();
            var cuotas = await connection.QueryAsync<Cuota>(query, new { ComprobanteId = comprobanteId });
            return cuotas;
        }

        public async Task CreateCuotasAsync(int comprobanteId, List<Cuota> cuotas, IDbConnection connection, IDbTransaction transaction)
        {
            const string query = @"
                INSERT INTO cuotas
                (comprobante_id, fecha, importe, estado)
                VALUES
                (@Comprobante_Id, @Fecha, @Importe, @Estado)";

            foreach (var cuota in cuotas)
            {
                cuota.Comprobante_Id = comprobanteId;
                await connection.ExecuteAsync(query, cuota, transaction);
            }
        }

        public async Task DeleteByComprobanteIdAsync(int comprobanteId, IDbConnection connection, IDbTransaction transaction)
        {
            const string query = "DELETE FROM cuotas WHERE comprobante_id = @ComprobanteId";
            await connection.ExecuteAsync(query, new { ComprobanteId = comprobanteId }, transaction);
        }

        public async Task<IEnumerable<CuotaListadoDto>> GetCuotasByZonaAsync(int? zonaId)
        {
            // Query para cuotas normales + cuota cero (contra entrega)
            var query = @"
                SELECT
                    cu.id AS Id,
                    cu.comprobante_id AS ComprobanteId,
                    c.numeroComprobante AS NumeroComprobante,
                    c.fecha AS FechaComprobante,
                    cl.Id AS ClienteId,
                    cl.Nombre AS ClienteNombre,
                    z.id AS ZonaId,
                    z.descripcion AS ZonaNombre,
                    cu.fecha AS FechaCuota,
                    COALESCE(cu.importe, 0) AS Importe,
                    COALESCE(cu.importe_pagado, 0) AS ImportePagado,
                    cu.estado AS Estado,
                    0 AS EsCuotaCero
                FROM cuotas cu
                INNER JOIN comprobantes c ON cu.comprobante_id = c.id
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                LEFT JOIN zonas z ON cl.Zona_Id = z.id
                WHERE 1=1";

            if (zonaId.HasValue)
            {
                query += " AND z.id = @ZonaId";
            }

            // Agregar cuota cero (contra entrega) de comprobantes que tengan ContraEntrega > 0
            query += @"
                UNION ALL
                SELECT
                    c.id * -1 AS Id,
                    c.id AS ComprobanteId,
                    c.numeroComprobante AS NumeroComprobante,
                    c.fecha AS FechaComprobante,
                    cl.Id AS ClienteId,
                    cl.Nombre AS ClienteNombre,
                    z.id AS ZonaId,
                    z.descripcion AS ZonaNombre,
                    c.fecha AS FechaCuota,
                    COALESCE(c.ContraEntrega, 0) AS Importe,
                    COALESCE(c.ContraEntregaPagado, 0) AS ImportePagado,
                    CASE WHEN COALESCE(c.ContraEntregaPagado, 0) >= COALESCE(c.ContraEntrega, 0) THEN 'PAG' ELSE 'PEN' END AS Estado,
                    1 AS EsCuotaCero
                FROM comprobantes c
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                LEFT JOIN zonas z ON cl.Zona_Id = z.id
                WHERE COALESCE(c.ContraEntrega, 0) > 0";

            if (zonaId.HasValue)
            {
                query += " AND z.id = @ZonaId";
            }

            query += " ORDER BY FechaCuota, ClienteNombre, EsCuotaCero, Id";

            using var connection = _context.CreateConnection();
            var cuotas = await connection.QueryAsync<CuotaListadoDto>(query, new { ZonaId = zonaId });
            return cuotas;
        }

        public async Task<bool> UpdateImportePagadoAsync(int cuotaId, decimal importePagado)
        {
            const string query = @"
                UPDATE cuotas
                SET importe_pagado = @ImportePagado,
                    estado = CASE WHEN @ImportePagado >= importe THEN 'PAG' ELSE 'PEN' END
                WHERE id = @CuotaId";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { CuotaId = cuotaId, ImportePagado = importePagado });
            return rowsAffected > 0;
        }

        public async Task<bool> UpdateContraEntregaPagadoAsync(int comprobanteId, decimal importePagado)
        {
            const string query = @"
                UPDATE comprobantes
                SET ContraEntregaPagado = @ImportePagado
                WHERE id = @ComprobanteId";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { ComprobanteId = comprobanteId, ImportePagado = importePagado });
            return rowsAffected > 0;
        }
    }
}
