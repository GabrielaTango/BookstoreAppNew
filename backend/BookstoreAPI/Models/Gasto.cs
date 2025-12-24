namespace BookstoreAPI.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public string NroComprobante { get; set; } = string.Empty;
        public decimal Importe { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
