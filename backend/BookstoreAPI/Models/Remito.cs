namespace BookstoreAPI.Models
{
    public class Remito
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public int TransporteId { get; set; }
        public int CantidadBultos { get; set; }
        public decimal ValorDeclarado { get; set; }
        public string? Observaciones { get; set; }

        // Campos enriquecidos para consultas (JOINs)
        public string? ClienteNombre { get; set; }
        public string? ClienteDomicilio { get; set; }
        public string? ClienteLocalidad { get; set; }
        public string? ClienteProvincia { get; set; }
        public string? ClienteCuit { get; set; }
        public string? ClienteCodigoPostal { get; set; }
        public string? TransporteNombre { get; set; }
        public string? TransporteDireccion { get; set; }
        public string? TransporteLocalidad { get; set; }
        public string? TransporteCuit { get; set; }
    }
}
