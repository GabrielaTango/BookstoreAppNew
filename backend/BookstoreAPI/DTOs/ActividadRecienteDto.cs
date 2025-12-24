namespace BookstoreAPI.DTOs
{
    public class ActividadRecienteDto
    {
        public string Tipo { get; set; } = string.Empty;
        public string Icono { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
