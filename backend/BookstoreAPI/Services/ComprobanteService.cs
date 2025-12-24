using BookstoreAPI.DTOs;
using BookstoreAPI.Models;
using BookstoreAPI.Models.Afip;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services.Afip;
using Microsoft.Extensions.Options;

namespace BookstoreAPI.Services
{
    public class ComprobanteService : IComprobanteService
    {
        private readonly IComprobanteRepository _comprobanteRepository;
        private readonly IAfipFacturacionService _afipService;
        private readonly ILogger<ComprobanteService> _logger;
        private readonly IOptions<AfipConfig> _config;
        public ComprobanteService(
            IComprobanteRepository comprobanteRepository,
            IAfipFacturacionService afipService,
            ILogger<ComprobanteService> logger,
            IOptions<AfipConfig> config)
        {
            _comprobanteRepository = comprobanteRepository;
            _afipService = afipService;
            _logger = logger;
            _config = config;
        }

        public async Task<IEnumerable<ComprobanteConDetallesDto>> GetAllAsync()
        {
            return await _comprobanteRepository.GetAllAsync();
        }

        public async Task<ComprobanteConDetallesDto?> GetByIdAsync(int id)
        {
            return await _comprobanteRepository.GetByIdAsync(id);
        }

        public async Task<ComprobanteConDetallesDto> CreateAsync(CreateComprobanteDto dto)
        {
            var comprobante = new Comprobante
            {
                Cliente_Id = dto.Cliente_Id,
                Fecha = dto.Fecha,
                TipoComprobante = dto.TipoComprobante,
                Total = dto.Total,
                Bonificacion = dto.Bonificacion,
                PorcentajeBonif = dto.PorcentajeBonif,
                Anticipo = dto.Anticipo,
                Cuotas = dto.Cuotas,
                ValorCuota = dto.ValorCuota,
                Vendedor_Id = dto.Vendedor_Id
            };

            var detalles = dto.Detalles.Select(d => new ComprobanteDetalle
            {
                Articulo_Id = d.Articulo_Id,
                Cantidad = d.Cantidad,
                Precio_Unitario = d.Precio_Unitario,
                Subtotal = d.Subtotal
            }).ToList();

            // Solicitar CAE a AFIP
            try
            {
                _logger.LogInformation("Solicitando CAE a AFIP para comprobante");
                var caeResponse = await _afipService.SolicitarCAEAsync(comprobante, detalles);

                if (caeResponse.Success)
                {
                    comprobante.CAE = caeResponse.CAE;
                    comprobante.VTO = caeResponse.CAEVencimiento;
                    comprobante.NumeroComprobante = caeResponse.NumeroComprobante;
                    comprobante.TipoComprobante = "FC"; // Se puede ajustar según el tipo
                    _logger.LogInformation("CAE obtenido exitosamente: {CAE}", caeResponse.CAE);
                }
                else
                {
                    var errores = string.Join(", ", caeResponse.Errores);
                    _logger.LogError("Error al obtener CAE de AFIP: {Errores}", errores);
                    throw new Exception($"Error al obtener CAE de AFIP: {errores}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al solicitar CAE a AFIP");
                throw new Exception($"Error al solicitar CAE: {ex.Message}", ex);
            }

            var createdComprobante = await _comprobanteRepository.CreateAsync(comprobante, detalles);

            return await _comprobanteRepository.GetByIdAsync(createdComprobante.Id)
                ?? throw new Exception("Error al recuperar el comprobante creado");
        }

        public async Task<ComprobanteConDetallesDto?> UpdateAsync(int id, UpdateComprobanteDto dto)
        {
            var comprobante = new Comprobante
            {
                Cliente_Id = dto.Cliente_Id,
                Fecha = dto.Fecha,
                TipoComprobante = dto.TipoComprobante,
                NumeroComprobante = dto.NumeroComprobante,
                Total = dto.Total,
                CAE = dto.CAE,
                VTO = dto.VTO,
                Bonificacion = dto.Bonificacion,
                PorcentajeBonif = dto.PorcentajeBonif,
                Anticipo = dto.Anticipo,
                Cuotas = dto.Cuotas,
                ValorCuota = dto.ValorCuota,
                Vendedor_Id = dto.Vendedor_Id
            };

            var detalles = dto.Detalles.Select(d => new ComprobanteDetalle
            {
                Articulo_Id = d.Articulo_Id,
                Cantidad = d.Cantidad,
                Precio_Unitario = d.Precio_Unitario,
                Subtotal = d.Subtotal
            }).ToList();

            var updatedComprobante = await _comprobanteRepository.UpdateAsync(id, comprobante, detalles);

            if (updatedComprobante == null)
                return null;

            return await _comprobanteRepository.GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _comprobanteRepository.DeleteAsync(id);
        }
    }
}
