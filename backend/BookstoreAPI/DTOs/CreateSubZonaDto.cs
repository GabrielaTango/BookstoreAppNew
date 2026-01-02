using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class CreateSubZonaDto
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(25, ErrorMessage = "El código no puede exceder los 25 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [MaxLength(10, ErrorMessage = "El código postal no puede exceder los 10 caracteres")]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [MaxLength(100, ErrorMessage = "La localidad no puede exceder los 100 caracteres")]
        public string Localidad { get; set; } = string.Empty;
    }

    public class UpdateSubZonaDto
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(25, ErrorMessage = "El código no puede exceder los 25 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [MaxLength(100, ErrorMessage = "La descripción no puede exceder los 100 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [MaxLength(10, ErrorMessage = "El código postal no puede exceder los 10 caracteres")]
        public string CodigoPostal { get; set; } = string.Empty;

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [MaxLength(100, ErrorMessage = "La localidad no puede exceder los 100 caracteres")]
        public string Localidad { get; set; } = string.Empty;
    }
}
