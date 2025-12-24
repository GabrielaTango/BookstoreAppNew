using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class CategoriaGastoService : ICategoriaGastoService
    {
        private readonly ICategoriaGastoRepository _categoriaRepository;

        public CategoriaGastoService(ICategoriaGastoRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaGasto>> GetAllCategoriasAsync()
        {
            return await _categoriaRepository.GetAllAsync();
        }

        public async Task<IEnumerable<CategoriaGasto>> GetCategoriasActivasAsync()
        {
            return await _categoriaRepository.GetActivasAsync();
        }

        public async Task<CategoriaGasto?> GetCategoriaByIdAsync(int id)
        {
            return await _categoriaRepository.GetByIdAsync(id);
        }

        public async Task<CategoriaGasto> CreateCategoriaAsync(CreateCategoriaGastoDto createDto)
        {
            var categoria = new CategoriaGasto
            {
                Nombre = createDto.Nombre,
                Descripcion = createDto.Descripcion,
                Activo = true
            };

            var id = await _categoriaRepository.CreateAsync(categoria);
            categoria.Id = id;
            return categoria;
        }

        public async Task<CategoriaGasto?> UpdateCategoriaAsync(int id, UpdateCategoriaGastoDto updateDto)
        {
            var exists = await _categoriaRepository.ExistsAsync(id);
            if (!exists)
            {
                return null;
            }

            var categoria = new CategoriaGasto
            {
                Id = id,
                Nombre = updateDto.Nombre,
                Descripcion = updateDto.Descripcion,
                Activo = updateDto.Activo
            };

            await _categoriaRepository.UpdateAsync(id, categoria);
            return categoria;
        }

        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var exists = await _categoriaRepository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }

            return await _categoriaRepository.DeleteAsync(id);
        }
    }
}
