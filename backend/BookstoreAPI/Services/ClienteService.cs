using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using BookstoreAPI.Repositories;

namespace BookstoreAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientesAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<Cliente?> GetClienteByCodigoAsync(string codigo)
        {
            return await _clienteRepository.GetByCodigoAsync(codigo);
        }

        public async Task<Cliente> CreateClienteAsync(CreateClienteDto createDto)
        {
            var cliente = new Cliente
            {
                Codigo = createDto.Codigo,
                Nombre = createDto.Nombre,
                Zona_Id = createDto.Zona_Id,
                SubZona_Id = createDto.SubZona_Id,
                Vendedor_Id = createDto.Vendedor_Id,
                DomicilioComercial = createDto.DomicilioComercial,
                DomicilioParticular = createDto.DomicilioParticular,
                Provincia_Id = createDto.Provincia_Id,
                CodigoPostal = createDto.CodigoPostal,
                FechaAlta = DateTime.Now,
                SoloContado = createDto.SoloContado,
                Telefono = createDto.Telefono,
                TelefonoMovil = createDto.TelefonoMovil,
                EMail = createDto.EMail,
                Contacto = createDto.Contacto,
                TipoDocumento = createDto.TipoDocumento,
                NroDocumento = createDto.NroDocumento,
                NroIIBB = createDto.NroIIBB,
                CategoriaIva = createDto.CategoriaIva,
                CondicionPago = createDto.CondicionPago,
                Descuento = createDto.Descuento,
                Observaciones = createDto.Observaciones,
                TipoDocArca = createDto.TipoDocArca
            };

            var id = await _clienteRepository.CreateAsync(cliente);
            cliente.Id = id;
            return cliente;
        }

        public async Task<Cliente?> UpdateClienteAsync(int id, UpdateClienteDto updateDto)
        {
            var existingCliente = await _clienteRepository.GetByIdAsync(id);
            if (existingCliente == null)
            {
                return null;
            }

            existingCliente.Codigo = updateDto.Codigo;
            existingCliente.Nombre = updateDto.Nombre;
            existingCliente.Zona_Id = updateDto.Zona_Id;
            existingCliente.SubZona_Id = updateDto.SubZona_Id;
            existingCliente.Vendedor_Id = updateDto.Vendedor_Id;
            existingCliente.DomicilioComercial = updateDto.DomicilioComercial;
            existingCliente.DomicilioParticular = updateDto.DomicilioParticular;
            existingCliente.Provincia_Id = updateDto.Provincia_Id;
            existingCliente.CodigoPostal = updateDto.CodigoPostal;
            existingCliente.FechaInha = updateDto.FechaInha;
            existingCliente.SoloContado = updateDto.SoloContado;
            existingCliente.Telefono = updateDto.Telefono;
            existingCliente.TelefonoMovil = updateDto.TelefonoMovil;
            existingCliente.EMail = updateDto.EMail;
            existingCliente.Contacto = updateDto.Contacto;
            existingCliente.TipoDocumento = updateDto.TipoDocumento;
            existingCliente.NroDocumento = updateDto.NroDocumento;
            existingCliente.NroIIBB = updateDto.NroIIBB;
            existingCliente.CategoriaIva = updateDto.CategoriaIva;
            existingCliente.CondicionPago = updateDto.CondicionPago;
            existingCliente.Descuento = updateDto.Descuento;
            existingCliente.Observaciones = updateDto.Observaciones;
            existingCliente.TipoDocArca = updateDto.TipoDocArca;

            var updated = await _clienteRepository.UpdateAsync(id, existingCliente);
            return updated ? existingCliente : null;
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            var exists = await _clienteRepository.ExistsAsync(id);
            if (!exists)
            {
                return false;
            }

            return await _clienteRepository.DeleteAsync(id);
        }
    }
}
