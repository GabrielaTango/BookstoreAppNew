using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class CreateRemitoDto
    {
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El transporte es obligatorio")]
        public int TransporteId { get; set; }

        [Required(ErrorMessage = "La cantidad de bultos es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de bultos debe ser mayor a 0")]
        public int CantidadBultos { get; set; }

        [Required(ErrorMessage = "El valor declarado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor declarado debe ser mayor a 0")]
        public decimal ValorDeclarado { get; set; }

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string? Observaciones { get; set; }
    }

    public class UpdateRemitoDto
    {
        [Required(ErrorMessage = "El cliente es obligatorio")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El transporte es obligatorio")]
        public int TransporteId { get; set; }

        [Required(ErrorMessage = "La cantidad de bultos es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de bultos debe ser mayor a 0")]
        public int CantidadBultos { get; set; }

        [Required(ErrorMessage = "El valor declarado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El valor declarado debe ser mayor a 0")]
        public decimal ValorDeclarado { get; set; }

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string? Observaciones { get; set; }
    }
}
