using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task<Cliente?> GetByCodigoAsync(string codigo);
        Task<int> CreateAsync(Cliente cliente);
        Task<bool> UpdateAsync(int id, Cliente cliente);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
