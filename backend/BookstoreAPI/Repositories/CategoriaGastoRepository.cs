using Dapper;
using BookstoreAPI.Data;
using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public class CategoriaGastoRepository : ICategoriaGastoRepository
    {
        private readonly DapperContext _context;

        public CategoriaGastoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaGasto>> GetAllAsync()
        {
            const string query = @"
                SELECT Id, Nombre, Descripcion, Activo
                FROM categorias_gasto
                ORDER BY Nombre";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CategoriaGasto>(query);
        }

        public async Task<IEnumerable<CategoriaGasto>> GetActivasAsync()
        {
            const string query = @"
                SELECT Id, Nombre, Descripcion, Activo
                FROM categorias_gasto
                WHERE Activo = 1
                ORDER BY Nombre";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CategoriaGasto>(query);
        }

        public async Task<CategoriaGasto?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT Id, Nombre, Descripcion, Activo
                FROM categorias_gasto
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CategoriaGasto>(query, new { Id = id });
        }

        public async Task<CategoriaGasto?> GetByNombreAsync(string nombre)
        {
            const string query = @"
                SELECT Id, Nombre, Descripcion, Activo
                FROM categorias_gasto
                WHERE Nombre = @Nombre";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<CategoriaGasto>(query, new { Nombre = nombre });
        }

        public async Task<int> CreateAsync(CategoriaGasto categoria)
        {
            const string query = @"
                INSERT INTO categorias_gasto (Nombre, Descripcion, Activo)
                VALUES (@Nombre, @Descripcion, @Activo);
                SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, categoria);
        }

        public async Task<bool> UpdateAsync(int id, CategoriaGasto categoria)
        {
            const string query = @"
                UPDATE categorias_gasto
                SET Nombre = @Nombre,
                    Descripcion = @Descripcion,
                    Activo = @Activo
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new
            {
                Id = id,
                categoria.Nombre,
                categoria.Descripcion,
                categoria.Activo
            });
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "DELETE FROM categorias_gasto WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            const string query = "SELECT COUNT(1) FROM categorias_gasto WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id });
            return count > 0;
        }
    }
}
