using BookstoreAPI.Data;
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
    }
}
