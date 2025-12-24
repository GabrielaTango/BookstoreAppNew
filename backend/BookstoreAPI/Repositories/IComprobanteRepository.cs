using BookstoreAPI.DTOs;
using BookstoreAPI.Models;

namespace BookstoreAPI.Repositories
{
    public interface IComprobanteRepository
    {
        Task<IEnumerable<ComprobanteConDetallesDto>> GetAllAsync();
        Task<ComprobanteConDetallesDto?> GetByIdAsync(int id);
        Task<Comprobante?> GetComprobanteByIdAsync(int id);
        Task<Comprobante> CreateAsync(Comprobante comprobante, List<ComprobanteDetalle> detalles);
        Task<Comprobante?> UpdateAsync(int id, Comprobante comprobante, List<ComprobanteDetalle> detalles);
        Task<bool> DeleteAsync(int id);
        Task<List<ComprobanteDetalle>> GetDetallesByComprobanteIdAsync(int comprobanteId);
        Task<IEnumerable<IvaVentasDto>> GetIvaVentasAsync(DateTime fechaDesde, DateTime fechaHasta);
    }
}
