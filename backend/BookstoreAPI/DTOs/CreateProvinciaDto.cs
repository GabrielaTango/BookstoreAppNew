using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class CreateProvinciaDto
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(25, ErrorMessage = "El código no puede exceder los 25 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateProvinciaDto
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(25, ErrorMessage = "El código no puede exceder los 25 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;
    }
}
