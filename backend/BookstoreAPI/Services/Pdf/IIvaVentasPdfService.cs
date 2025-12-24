using BookstoreAPI.DTOs;

namespace BookstoreAPI.Services.Pdf
{
    public interface IIvaVentasPdfService
    {
        byte[] GenerarPdf(List<IvaVentasDto> ventas, DateTime fechaDesde, DateTime fechaHasta);
    }
}
