using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<Articulo>> GetAllAsync();
        Task<Articulo?> GetByIdAsync(int id);
        Task<Articulo?> GetByCodigoAsync(string codigo);
        Task<int> CreateAsync(Articulo articulo);
        Task<bool> UpdateAsync(int id, Articulo articulo);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
