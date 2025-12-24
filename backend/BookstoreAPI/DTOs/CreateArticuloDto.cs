using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class CreateArticuloDto
    {
        [StringLength(25, ErrorMessage = "El código no puede exceder 25 caracteres")]
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(100, ErrorMessage = "La descripción no puede exceder 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [StringLength(13, ErrorMessage = "El código de barras no puede exceder 13 caracteres")]
        public string? CodBarras { get; set; }

        [StringLength(2000, ErrorMessage = "Las observaciones no pueden exceder 2000 caracteres")]
        public string? Observaciones { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Los tomos deben ser un número positivo")]
        public int? Tomos { get; set; }

        [StringLength(50, ErrorMessage = "El tema no puede exceder 50 caracteres")]
        public string? Tema { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo")]
        public decimal? Precio { get; set; }
    }
}
