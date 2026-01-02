using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface IRemitoRepository
    {
        Task<IEnumerable<Remito>> GetAllAsync();
        Task<Remito?> GetByIdAsync(int id);
        Task<Remito> CreateAsync(Remito remito);
        Task<Remito?> UpdateAsync(int id, Remito remito);
        Task<bool> DeleteAsync(int id);
        Task<string> GetNextNumeroAsync();
    }
}
