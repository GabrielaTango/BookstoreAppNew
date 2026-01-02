using BookstoreAPI.Models;

namespace BookstoreAPI.Services.Pdf
{
    public interface IRemitoPdfService
    {
        byte[] GenerarRemitoPdf(Remito remito);
        byte[] GenerarEtiquetasPdf(Remito remito);
        byte[] GenerarRemitoCompletoConEtiquetas(Remito remito);
    }
}
