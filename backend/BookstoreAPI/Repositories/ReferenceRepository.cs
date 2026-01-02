using BookstoreAPI.Data;
using BookstoreAPI.Models;
using Dapper;

namespace BookstoreAPI.Repositories
{
    public class ReferenceRepository : IReferenceRepository
    {
        private readonly DapperContext _context;

        public ReferenceRepository(DapperContext context)
        {
            _context = context;
        }

        // Zona - CRUD Operations
        public async Task<IEnumerable<Zona>> GetAllZonasAsync()
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM zonas ORDER BY descripcion";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Zona>(query);
        }

        public async Task<Zona?> GetZonaByIdAsync(int id)
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM zonas WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Zona>(query, new { Id = id });
        }

        public async Task<Zona> CreateZonaAsync(Zona zona)
        {
            const string query = @"
                INSERT INTO zonas (codigo, descripcion)
                VALUES (@Codigo, @Descripcion);
                SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, zona);
            zona.Id = id;
            return zona;
        }

        public async Task<Zona?> UpdateZonaAsync(int id, Zona zona)
        {
            const string query = @"
                UPDATE zonas
                SET codigo = @Codigo, descripcion = @Descripcion
                WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id, zona.Codigo, zona.Descripcion });
            if (affectedRows == 0) return null;
            zona.Id = id;
            return zona;
        }

        public async Task<bool> DeleteZonaAsync(int id)
        {
            const string query = "DELETE FROM zonas WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        // SubZona - CRUD Operations
        public async Task<IEnumerable<SubZona>> GetAllSubZonasAsync()
        {
            const string query = @"
                SELECT s.id AS Id, s.codigo AS Codigo, s.descripcion AS Descripcion,
                       s.provincia_id AS ProvinciaId, s.codigo_postal AS CodigoPostal, s.localidad AS Localidad,
                       p.descripcion AS ProvinciaDescripcion
                FROM subzonas s
                LEFT JOIN provincias p ON s.provincia_id = p.id
                ORDER BY s.descripcion";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SubZona>(query);
        }

        public async Task<SubZona?> GetSubZonaByIdAsync(int id)
        {
            const string query = @"
                SELECT s.id AS Id, s.codigo AS Codigo, s.descripcion AS Descripcion,
                       s.provincia_id AS ProvinciaId, s.codigo_postal AS CodigoPostal, s.localidad AS Localidad,
                       p.descripcion AS ProvinciaDescripcion
                FROM subzonas s
                LEFT JOIN provincias p ON s.provincia_id = p.id
                WHERE s.id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SubZona>(query, new { Id = id });
        }

        public async Task<SubZona> CreateSubZonaAsync(SubZona subZona)
        {
            const string query = @"
                INSERT INTO subzonas (codigo, descripcion, provincia_id, codigo_postal, localidad)
                VALUES (@Codigo, @Descripcion, @ProvinciaId, @CodigoPostal, @Localidad);
                SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, subZona);
            subZona.Id = id;
            return subZona;
        }

        public async Task<SubZona?> UpdateSubZonaAsync(int id, SubZona subZona)
        {
            const string query = @"
                UPDATE subzonas
                SET codigo = @Codigo, descripcion = @Descripcion,
                    provincia_id = @ProvinciaId, codigo_postal = @CodigoPostal, localidad = @Localidad
                WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new {
                Id = id,
                subZona.Codigo,
                subZona.Descripcion,
                subZona.ProvinciaId,
                subZona.CodigoPostal,
                subZona.Localidad
            });
            if (affectedRows == 0) return null;
            subZona.Id = id;
            return subZona;
        }

        public async Task<bool> DeleteSubZonaAsync(int id)
        {
            const string query = "DELETE FROM subzonas WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        // Provincia - CRUD Operations
        public async Task<IEnumerable<Provincia>> GetAllProvinciasAsync()
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM provincias ORDER BY descripcion";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Provincia>(query);
        }

        public async Task<Provincia?> GetProvinciaByIdAsync(int id)
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM provincias WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Provincia>(query, new { Id = id });
        }

        public async Task<Provincia> CreateProvinciaAsync(Provincia provincia)
        {
            const string query = @"
                INSERT INTO provincias (codigo, descripcion)
                VALUES (@Codigo, @Descripcion);
                SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, provincia);
            provincia.Id = id;
            return provincia;
        }

        public async Task<Provincia?> UpdateProvinciaAsync(int id, Provincia provincia)
        {
            const string query = @"
                UPDATE provincias
                SET codigo = @Codigo, descripcion = @Descripcion
                WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id, provincia.Codigo, provincia.Descripcion });
            if (affectedRows == 0) return null;
            provincia.Id = id;
            return provincia;
        }

        public async Task<bool> DeleteProvinciaAsync(int id)
        {
            const string query = "DELETE FROM provincias WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        // Vendedor - CRUD Operations
        public async Task<IEnumerable<Vendedor>> GetAllVendedoresAsync()
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM vendedores ORDER BY descripcion";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Vendedor>(query);
        }

        public async Task<Vendedor?> GetVendedorByIdAsync(int id)
        {
            const string query = "SELECT id AS Id, codigo AS Codigo, descripcion AS Descripcion FROM vendedores WHERE id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Vendedor>(query, new { Id = id });
        }

        public async Task<Vendedor> CreateVendedorAsync(Vendedor vendedor)
        {
            const string query = @"
                INSERT INTO vendedores (codigo, descripcion)
                VALUES (@Codigo, @Descripcion);
                SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, vendedor);
            vendedor.Id = id;
            return vendedor;
        }

        public async Task<Vendedor?> UpdateVendedorAsync(int id, Vendedor vendedor)
        {
            const string query = @"
                UPDATE vendedores
                SET codigo = @Codigo, descripcion = @Descripcion
                WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id, vendedor.Codigo, vendedor.Descripcion });
            if (affectedRows == 0) return null;
            vendedor.Id = id;
            return vendedor;
        }

        public async Task<bool> DeleteVendedorAsync(int id)
        {
            const string query = "DELETE FROM vendedores WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

        // Transporte - CRUD Operations
        public async Task<IEnumerable<Transporte>> GetAllTransportesAsync()
        {
            const string query = @"
                SELECT t.id AS Id, t.codigo AS Codigo, t.nombre AS Nombre,
                       t.direccion AS Direccion, t.localidad AS Localidad,
                       t.provincia_id AS ProvinciaId, t.cuit AS Cuit,
                       p.descripcion AS ProvinciaDescripcion
                FROM transportes t
                LEFT JOIN provincias p ON t.provincia_id = p.id
                ORDER BY t.nombre";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Transporte>(query);
        }

        public async Task<Transporte?> GetTransporteByIdAsync(int id)
        {
            const string query = @"
                SELECT t.id AS Id, t.codigo AS Codigo, t.nombre AS Nombre,
                       t.direccion AS Direccion, t.localidad AS Localidad,
                       t.provincia_id AS ProvinciaId, t.cuit AS Cuit,
                       p.descripcion AS ProvinciaDescripcion
                FROM transportes t
                LEFT JOIN provincias p ON t.provincia_id = p.id
                WHERE t.id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Transporte>(query, new { Id = id });
        }

        public async Task<Transporte> CreateTransporteAsync(Transporte transporte)
        {
            const string query = @"
                INSERT INTO transportes (codigo, nombre, direccion, localidad, provincia_id, cuit)
                VALUES (@Codigo, @Nombre, @Direccion, @Localidad, @ProvinciaId, @Cuit);
                SELECT LAST_INSERT_ID();";
            using var connection = _context.CreateConnection();
            var id = await connection.ExecuteScalarAsync<int>(query, transporte);
            transporte.Id = id;
            return transporte;
        }

        public async Task<Transporte?> UpdateTransporteAsync(int id, Transporte transporte)
        {
            const string query = @"
                UPDATE transportes
                SET codigo = @Codigo, nombre = @Nombre, direccion = @Direccion,
                    localidad = @Localidad, provincia_id = @ProvinciaId, cuit = @Cuit
                WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new {
                Id = id,
                transporte.Codigo,
                transporte.Nombre,
                transporte.Direccion,
                transporte.Localidad,
                transporte.ProvinciaId,
                transporte.Cuit
            });
            if (affectedRows == 0) return null;
            transporte.Id = id;
            return transporte;
        }

        public async Task<bool> DeleteTransporteAsync(int id)
        {
            const string query = "DELETE FROM transportes WHERE id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}
