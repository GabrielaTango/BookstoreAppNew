namespace BookstoreAPI.Models.Afip
{
    public class AfipConfig
    {
        public string CUIT { get; set; } = string.Empty;
        public string PfxPath { get; set; } = string.Empty;
        public string PfxPassword { get; set; } = string.Empty;
        public string WsaaUrl { get; set; } = string.Empty;
        public string WsfevUrl { get; set; } = string.Empty;
        public int PuntoVenta { get; set; }
        public bool IsProduction { get; set; }
    }
}
