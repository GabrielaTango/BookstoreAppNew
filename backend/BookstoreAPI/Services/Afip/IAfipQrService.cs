using BookstoreAPI.Models;

namespace BookstoreAPI.Services.Afip
{
    public interface IAfipQrService
    {
        /// <summary>
        /// Genera el código QR de AFIP en formato base64
        /// </summary>
        string GenerarQrBase64(Comprobante comprobante, Cliente cliente);

        /// <summary>
        /// Genera los bytes del código QR de AFIP
        /// </summary>
        byte[] GenerarQrBytes(Comprobante comprobante, Cliente cliente);
    }
}
