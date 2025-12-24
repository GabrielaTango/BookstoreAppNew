using BookstoreAPI.DTOs;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services;
using BookstoreAPI.Services.Pdf;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComprobantesController : ControllerBase
    {
        private readonly IComprobanteService _comprobanteService;
        private readonly IComprobantePdfService _pdfService;
        private readonly ICuotaPdfService _cuotaPdfService;
        private readonly IIvaVentasPdfService _ivaVentasPdfService;
        private readonly IComprobanteRepository _comprobanteRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly ICuotaRepository _cuotaRepository;
        private readonly ILogger<ComprobantesController> _logger;

        public ComprobantesController(
            IComprobanteService comprobanteService,
            IComprobantePdfService pdfService,
            ICuotaPdfService cuotaPdfService,
            IIvaVentasPdfService ivaVentasPdfService,
            IComprobanteRepository comprobanteRepository,
            IClienteRepository clienteRepository,
            ICuotaRepository cuotaRepository,
            ILogger<ComprobantesController> logger)
        {
            _comprobanteService = comprobanteService;
            _pdfService = pdfService;
            _cuotaPdfService = cuotaPdfService;
            _ivaVentasPdfService = ivaVentasPdfService;
            _comprobanteRepository = comprobanteRepository;
            _clienteRepository = clienteRepository;
            _cuotaRepository = cuotaRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var comprobantes = await _comprobanteService.GetAllAsync();
                return Ok(comprobantes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener comprobantes");
                return StatusCode(500, new { message = "Error al obtener comprobantes" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var comprobante = await _comprobanteService.GetByIdAsync(id);
                if (comprobante == null)
                    return NotFound(new { message = $"Comprobante con ID {id} no encontrado" });
                return Ok(comprobante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener comprobante por ID");
                return StatusCode(500, new { message = "Error al obtener comprobante" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComprobanteDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var comprobante = await _comprobanteService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = comprobante.Id }, comprobante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear comprobante");
                return StatusCode(500, new { message = "Error al crear comprobante", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComprobanteDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var comprobante = await _comprobanteService.UpdateAsync(id, dto);
                if (comprobante == null)
                    return NotFound(new { message = $"Comprobante con ID {id} no encontrado" });
                return Ok(comprobante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar comprobante");
                return StatusCode(500, new { message = "Error al actualizar comprobante", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _comprobanteService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Comprobante con ID {id} no encontrado" });
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar comprobante");
                return StatusCode(500, new { message = "Error al eliminar comprobante" });
            }
        }

        [HttpGet("{id}/pdf")]
        public async Task<IActionResult> GetPdf(int id)
        {
            try
            {
                // Obtener comprobante
                var comprobante = await _comprobanteRepository.GetComprobanteByIdAsync(id);
                if (comprobante == null)
                    return NotFound(new { message = $"Comprobante con ID {id} no encontrado" });

                // Obtener cliente
                var cliente = await _clienteRepository.GetByIdAsync(comprobante.Cliente_Id);
                if (cliente == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                // Obtener detalles del comprobante
                var detalles = await _comprobanteRepository.GetDetallesByComprobanteIdAsync(id);

                // Generar PDF
                var pdfBytes = _pdfService.GenerarPdf(comprobante, cliente, detalles);

                // Retornar PDF para abrir en el navegador
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar PDF del comprobante {Id}", id);
                return StatusCode(500, new { message = "Error al generar PDF del comprobante", error = ex.Message });
            }
        }

        [HttpGet("{id}/cupones-pdf")]
        public async Task<IActionResult> GetCuponesPdf(int id)
        {
            try
            {
                // Obtener comprobante
                var comprobante = await _comprobanteRepository.GetComprobanteByIdAsync(id);
                if (comprobante == null)
                    return NotFound(new { message = $"Comprobante con ID {id} no encontrado" });

                // Obtener cliente
                var cliente = await _clienteRepository.GetByIdAsync(comprobante.Cliente_Id);
                if (cliente == null)
                    return NotFound(new { message = "Cliente no encontrado" });

                // Obtener cuotas del comprobante
                var cuotas = await _cuotaRepository.GetByComprobanteIdAsync(id);
                var listaCuotas = cuotas.ToList();

                if (!listaCuotas.Any())
                    return NotFound(new { message = "No hay cuotas para este comprobante" });

                // Generar PDF de cupones
                var pdfBytes = _cuotaPdfService.GenerarCuponesPdf(comprobante, cliente, listaCuotas);

                // Retornar PDF para abrir en el navegador
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar PDF de cupones del comprobante {Id}", id);
                return StatusCode(500, new { message = "Error al generar PDF de cupones", error = ex.Message });
            }
        }

        [HttpGet("iva-ventas")]
        public async Task<IActionResult> GetIvaVentas([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            try
            {
                // Validar que la fecha desde no sea mayor que la fecha hasta
                if (fechaDesde > fechaHasta)
                    return BadRequest(new { message = "La fecha desde no puede ser mayor que la fecha hasta" });

                // Obtener datos de IVA ventas
                var ventas = await _comprobanteRepository.GetIvaVentasAsync(fechaDesde, fechaHasta);
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de IVA ventas");
                return StatusCode(500, new { message = "Error al obtener datos de IVA ventas", error = ex.Message });
            }
        }

        [HttpGet("iva-ventas-pdf")]
        public async Task<IActionResult> GetIvaVentasPdf([FromQuery] DateTime fechaDesde, [FromQuery] DateTime fechaHasta)
        {
            try
            {
                // Validar que la fecha desde no sea mayor que la fecha hasta
                if (fechaDesde > fechaHasta)
                    return BadRequest(new { message = "La fecha desde no puede ser mayor que la fecha hasta" });

                // Obtener datos de IVA ventas
                var ventas = await _comprobanteRepository.GetIvaVentasAsync(fechaDesde, fechaHasta);
                var listaVentas = ventas.ToList();

                if (!listaVentas.Any())
                    return NotFound(new { message = "No se encontraron ventas en el período especificado" });

                // Generar PDF
                var pdfBytes = _ivaVentasPdfService.GenerarPdf(listaVentas, fechaDesde, fechaHasta);

                // Retornar PDF para abrir en el navegador
                return File(pdfBytes, "application/pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al generar PDF de IVA ventas");
                return StatusCode(500, new { message = "Error al generar PDF de IVA ventas", error = ex.Message });
            }
        }
    }
}
