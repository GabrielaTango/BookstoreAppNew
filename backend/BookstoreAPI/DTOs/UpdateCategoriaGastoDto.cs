using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class UpdateCategoriaGastoDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "La descripción no puede exceder 255 caracteres")]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;
    }
}
