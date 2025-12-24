using BookstoreAPI.Data;
using BookstoreAPI.DTOs;
using Dapper;

namespace BookstoreAPI.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _context;

        public DashboardRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            const string query = @"
                SELECT
                    (SELECT COUNT(*) FROM comprobantes) AS TotalComprobantes,
                    (SELECT COUNT(*) FROM clientes) AS TotalClientes,
                    (SELECT COUNT(*) FROM articulos) AS TotalArticulos,
                    (SELECT COUNT(*) FROM comprobantes WHERE DATE(fecha) = CURDATE()) AS VentasHoy";

            using var connection = _context.CreateConnection();
            var stats = await connection.QueryFirstOrDefaultAsync<DashboardStatsDto>(query);

            return stats ?? new DashboardStatsDto();
        }

        public async Task<IEnumerable<ActividadRecienteDto>> GetActividadesRecientesAsync(int cantidad = 10)
        {
            const string query = @"
                SELECT
                    'Comprobante' AS Tipo,
                    'bi-receipt' AS Icono,
                    'primary' AS Color,
                    CONCAT('Comprobante ', COALESCE(c.numeroComprobante, CONCAT('#', c.id)), ' - ', cl.Nombre) AS Descripcion,
                    c.fecha AS Fecha
                FROM comprobantes c
                INNER JOIN clientes cl ON c.cliente_id = cl.Id
                ORDER BY c.fecha DESC, c.id DESC
                LIMIT @Cantidad";

            using var connection = _context.CreateConnection();
            var actividades = await connection.QueryAsync<ActividadRecienteDto>(query, new { Cantidad = cantidad });

            return actividades;
        }
    }
}
