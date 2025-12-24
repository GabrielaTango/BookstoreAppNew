namespace BookstoreAPI.Models.Afip
{
    public class AfipCAEResponse
    {
        public bool Success { get; set; }
        public string CAE { get; set; } = string.Empty;
        public DateTime? CAEVencimiento { get; set; }
        public string NumeroComprobante { get; set; } = string.Empty;
        public List<string> Errores { get; set; } = new List<string>();
        public List<string> Observaciones { get; set; } = new List<string>();
    }
}
