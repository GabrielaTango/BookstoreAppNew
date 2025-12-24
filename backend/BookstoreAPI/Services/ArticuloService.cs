using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;

        public ArticuloService(IArticuloRepository articuloRepository)
        {
            _articuloRepository = articuloRepository;
        }

        public async Task<IEnumerable<Articulo>> GetAllArticulosAsync()
        {
            return await _articuloRepository.GetAllAsync();
        }

        public async Task<Articulo?> GetArticuloByIdAsync(int id)
        {
            return await _articuloRepository.GetByIdAsync(id);
        }

        public async Task<Articulo?> GetArticuloByCodigoAsync(string codigo)
        {
            return await _articuloRepository.GetByCodigoAsync(codigo);
        }

        public async Task<Articulo> CreateArticuloAsync(CreateArticuloDto createDto)
        {
            var articulo = new Articulo
            {
                Codigo = createDto.Codigo,
                Descripcion = createDto.Descripcion,
                CodBarras = createDto.CodBarras,
                Observaciones = createDto.Observaciones,
                Tomos = createDto.Tomos,
                Tema = createDto.Tema,
                Precio = createDto.Precio
            };

            var id = await _articuloRepository.CreateAsync(articulo);
            articulo.Id = id;
            return articulo;
        }

        public async Task<Articulo?> UpdateArticuloAsync(int id, UpdateArticuloDto updateDto)
        {
            var existingArticulo = await _articuloRepository.GetByIdAsync(id);
            if (existingArticulo == null)
            {
                return null;
            }

            existingArticulo.Codigo = updateDto.Codigo;
            existingArticulo.Descripcion = updateDto.Descripcion;
            existingArticulo.CodBarras = updateDto.CodBarras;
            existingArticulo.Observaciones = updateDto.Observaciones;
            existingArticulo.Tomos = updateDto.Tomos;
            existingArticulo.Tema = updateDto.Tema;
            existingArticulo.Precio = updateDto.Precio;

            var updated = await _articuloRepository.UpdateAsync(id, existingArticulo);
            return updated ? existingArticulo : null;
        }

        public async Task<bool> DeleteArticuloAsync(int id)
        {
            var exists = await _articuloRepository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }

            return await _articuloRepository.DeleteAsync(id);
        }
    }
}
