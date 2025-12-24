using BookstoreAPI.Data;
using BookstoreAPI.Models;
using Dapper;

namespace BookstoreAPI.Repositories
{
    public class ArticuloRepository : IArticuloRepository
    {
        private readonly DapperContext _context;

        public ArticuloRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articulo>> GetAllAsync()
        {
            const string query = @"
                SELECT Id, Codigo, Descripcion, CodBarras, Observaciones,
                       Tomos, Tema, Precio
                FROM articulos
                ORDER BY Descripcion";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Articulo>(query);
        }

        public async Task<Articulo?> GetByIdAsync(int id)
        {
            const string query = @"
                SELECT Id, Codigo, Descripcion, CodBarras, Observaciones,
                       Tomos, Tema, Precio
                FROM articulos
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Articulo>(query, new { Id = id });
        }

        public async Task<Articulo?> GetByCodigoAsync(string codigo)
        {
            const string query = @"
                SELECT Id, Codigo, Descripcion, CodBarras, Observaciones,
                       Tomos, Tema, Precio
                FROM articulos
                WHERE Codigo = @Codigo";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Articulo>(query, new { Codigo = codigo });
        }

        public async Task<int> CreateAsync(Articulo articulo)
        {
            const string query = @"
                INSERT INTO articulos (
                    Codigo, Descripcion, CodBarras, Observaciones, Tomos, Tema, Precio
                ) VALUES (
                    @Codigo, @Descripcion, @CodBarras, @Observaciones, @Tomos, @Tema, @Precio
                );
                SELECT LAST_INSERT_ID();";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, articulo);
        }

        public async Task<bool> UpdateAsync(int id, Articulo articulo)
        {
            const string query = @"
                UPDATE articulos SET
                    Codigo = @Codigo,
                    Descripcion = @Descripcion,
                    CodBarras = @CodBarras,
                    Observaciones = @Observaciones,
                    Tomos = @Tomos,
                    Tema = @Tema,
                    Precio = @Precio
                WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new
            {
                Id = id,
                articulo.Codigo,
                articulo.Descripcion,
                articulo.CodBarras,
                articulo.Observaciones,
                articulo.Tomos,
                articulo.Tema,
                articulo.Precio
            });

            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string query = "DELETE FROM articulos WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            const string query = "SELECT COUNT(1) FROM articulos WHERE Id = @Id";

            using var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(query, new { Id = id });
            return count > 0;
        }
    }
}
