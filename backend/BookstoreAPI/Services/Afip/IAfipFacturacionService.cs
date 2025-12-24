using BookstoreAPI.Models;
using BookstoreAPI.Models.Afip;

namespace BookstoreAPI.Services.Afip
{
    public interface IAfipFacturacionService
    {
        Task<AfipCAEResponse> SolicitarCAEAsync(Comprobante comprobante, List<ComprobanteDetalle> detalles);
        Task<int> GetUltimoComprobanteAutorizadoAsync(int puntoVenta, int tipoComprobante);
    }
}
