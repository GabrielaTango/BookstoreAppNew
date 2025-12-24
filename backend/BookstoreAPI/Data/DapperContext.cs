using MySql.Data.MySqlClient;
using System.Data;

namespace BookstoreAPI.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public IDbConnection CreateConnection()
            => new MySqlConnection(_connectionString);
    }
}
