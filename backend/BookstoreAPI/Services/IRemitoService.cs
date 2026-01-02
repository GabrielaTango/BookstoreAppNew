using BookstoreAPI.DTOs;
using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IRemitoService
    {
        Task<IEnumerable<Remito>> GetAllAsync();
        Task<Remito?> GetByIdAsync(int id);
        Task<Remito> CreateAsync(CreateRemitoDto dto);
        Task<Remito?> UpdateAsync(int id, UpdateRemitoDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
