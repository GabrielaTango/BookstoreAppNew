namespace BookstoreAPI.DTOs
{
    public class IvaVentasDto
    {
        public DateTime Fecha { get; set; }
        public string? NumeroComprobante { get; set; }
        public string? Nombre { get; set; }
        public string? NroDocumento { get; set; }
        public decimal Total { get; set; }
    }
}
