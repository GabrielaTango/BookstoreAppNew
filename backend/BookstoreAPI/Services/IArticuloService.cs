using BookstoreAPI.DTOs;
using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IArticuloService
    {
        Task<IEnumerable<Articulo>> GetAllArticulosAsync();
        Task<Articulo?> GetArticuloByIdAsync(int id);
        Task<Articulo?> GetArticuloByCodigoAsync(string codigo);
        Task<Articulo> CreateArticuloAsync(CreateArticuloDto createDto);
        Task<Articulo?> UpdateArticuloAsync(int id, UpdateArticuloDto updateDto);
        Task<bool> DeleteArticuloAsync(int id);
    }
}
