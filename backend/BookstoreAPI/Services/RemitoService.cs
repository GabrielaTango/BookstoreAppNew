using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class RemitoService : IRemitoService
    {
        private readonly IRemitoRepository _remitoRepository;

        public RemitoService(IRemitoRepository remitoRepository)
        {
            _remitoRepository = remitoRepository;
        }

        public async Task<IEnumerable<Remito>> GetAllAsync()
        {
            return await _remitoRepository.GetAllAsync();
        }

        public async Task<Remito?> GetByIdAsync(int id)
        {
            return await _remitoRepository.GetByIdAsync(id);
        }

        public async Task<Remito> CreateAsync(CreateRemitoDto dto)
        {
            var numero = await _remitoRepository.GetNextNumeroAsync();

            var remito = new Remito
            {
                Numero = numero,
                Fecha = DateTime.Now,
                ClienteId = dto.ClienteId,
                TransporteId = dto.TransporteId,
                CantidadBultos = dto.CantidadBultos,
                ValorDeclarado = dto.ValorDeclarado,
                Observaciones = dto.Observaciones
            };

            return await _remitoRepository.CreateAsync(remito);
        }

        public async Task<Remito?> UpdateAsync(int id, UpdateRemitoDto dto)
        {
            var remito = new Remito
            {
                ClienteId = dto.ClienteId,
                TransporteId = dto.TransporteId,
                CantidadBultos = dto.CantidadBultos,
                ValorDeclarado = dto.ValorDeclarado,
                Observaciones = dto.Observaciones
            };

            return await _remitoRepository.UpdateAsync(id, remito);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _remitoRepository.DeleteAsync(id);
        }
    }
}
