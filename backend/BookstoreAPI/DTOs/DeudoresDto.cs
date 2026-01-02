namespace BookstoreAPI.DTOs
{
    public class DeudorItemDto
    {
        public int ComprobanteId { get; set; }
        public string NumeroComprobante { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string? CodigoVendedor { get; set; }
        public int CantidadCuotas { get; set; }
        public decimal TotalComprobante { get; set; }
        public decimal Saldo { get; set; }
        public decimal Anticipo { get; set; }
        public decimal ContraEntrega { get; set; }
        public decimal ContraEntregaPagado { get; set; }
        public List<CuotaDeudorDto> Cuotas { get; set; } = new();
    }

    public class CuotaDeudorDto
    {
        public int CuotaId { get; set; }
        public string Periodo { get; set; } = string.Empty; // formato MM/YYYY
        public decimal Importe { get; set; }
        public decimal ImportePagado { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class DeudoresReporteDto
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        public List<string> PeriodosCuotas { get; set; } = new(); // Lista de periodos únicos para columnas
        public List<DeudorItemDto> Deudores { get; set; } = new();
    }
}
