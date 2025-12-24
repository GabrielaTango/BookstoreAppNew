using BookstoreAPI.DTOs;
using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface ICategoriaGastoService
    {
        Task<IEnumerable<CategoriaGasto>> GetAllCategoriasAsync();
        Task<IEnumerable<CategoriaGasto>> GetCategoriasActivasAsync();
        Task<CategoriaGasto?> GetCategoriaByIdAsync(int id);
        Task<CategoriaGasto> CreateCategoriaAsync(CreateCategoriaGastoDto createDto);
        Task<CategoriaGasto?> UpdateCategoriaAsync(int id, UpdateCategoriaGastoDto updateDto);
        Task<bool> DeleteCategoriaAsync(int id);
    }
}
