namespace BookstoreAPI.DTOs
{
    public class CuotaListadoDto
    {
        public int Id { get; set; }
        public int ComprobanteId { get; set; }
        public string? NumeroComprobante { get; set; }
        public DateTime? FechaComprobante { get; set; }
        public int ClienteId { get; set; }
        public string? ClienteNombre { get; set; }
        public int? ZonaId { get; set; }
        public string? ZonaNombre { get; set; }
        public DateTime? FechaCuota { get; set; }
        public decimal Importe { get; set; }
        public decimal ImportePagado { get; set; }
        public string? Estado { get; set; }
        public bool EsCuotaCero { get; set; }
    }

    public class UpdateImportePagadoDto
    {
        public decimal ImportePagado { get; set; }
        public bool EsCuotaCero { get; set; }
        public int? ComprobanteId { get; set; }
    }
}
