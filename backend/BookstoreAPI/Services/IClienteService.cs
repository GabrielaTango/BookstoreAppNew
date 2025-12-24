using BookstoreAPI.DTOs;
using BookstoreAPI.Models;

namespace BookstoreAPI.Services
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientesAsync();
        Task<Cliente?> GetClienteByIdAsync(int id);
        Task<Cliente?> GetClienteByCodigoAsync(string codigo);
        Task<Cliente> CreateClienteAsync(CreateClienteDto createDto);
        Task<Cliente?> UpdateClienteAsync(int id, UpdateClienteDto updateDto);
        Task<bool> DeleteClienteAsync(int id);
    }
}
