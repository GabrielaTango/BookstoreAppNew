namespace BookstoreAPI.Models
{
    public class Articulo
    {
        public int Id { get; set; }
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public string? CodBarras { get; set; }
        public string? Observaciones { get; set; }
        public int? Tomos { get; set; }
        public string? Tema { get; set; }
        public decimal? Precio { get; set; }
    }
}
