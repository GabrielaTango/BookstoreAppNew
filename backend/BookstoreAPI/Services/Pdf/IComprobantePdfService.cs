using BookstoreAPI.Models;

namespace BookstoreAPI.Services.Pdf
{
    public interface IComprobantePdfService
    {
        /// <summary>
        /// Genera el PDF del comprobante con el QR de AFIP
        /// </summary>
        byte[] GenerarPdf(Comprobante comprobante, Cliente cliente, List<ComprobanteDetalle> detalles);
    }
}
