using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface ICategoriaGastoRepository
    {
        Task<IEnumerable<CategoriaGasto>> GetAllAsync();
        Task<IEnumerable<CategoriaGasto>> GetActivasAsync();
        Task<CategoriaGasto?> GetByIdAsync(int id);
        Task<CategoriaGasto?> GetByNombreAsync(string nombre);
        Task<int> CreateAsync(CategoriaGasto categoria);
        Task<bool> UpdateAsync(int id, CategoriaGasto categoria);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
