using System.ComponentModel.DataAnnotations;

namespace BookstoreAPI.DTOs
{
    public class CreateClienteDto
    {
        public string? Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        public int? Zona_Id { get; set; }
        public int? SubZona_Id { get; set; }
        public int? Vendedor_Id { get; set; }

        [StringLength(200, ErrorMessage = "El domicilio comercial no puede exceder 200 caracteres")]
        public string? DomicilioComercial { get; set; }

        [StringLength(200, ErrorMessage = "El domicilio particular no puede exceder 200 caracteres")]
        public string? DomicilioParticular { get; set; }

        public int? Provincia_Id { get; set; }

        [StringLength(20, ErrorMessage = "El código postal no puede exceder 20 caracteres")]
        public string? CodigoPostal { get; set; }

        public bool SoloContado { get; set; }

        [StringLength(50, ErrorMessage = "El teléfono no puede exceder 50 caracteres")]
        public string? Telefono { get; set; }

        [StringLength(50, ErrorMessage = "El teléfono móvil no puede exceder 50 caracteres")]
        public string? TelefonoMovil { get; set; }

        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string? EMail { get; set; }

        [StringLength(100, ErrorMessage = "El contacto no puede exceder 100 caracteres")]
        public string? Contacto { get; set; }

        [StringLength(50, ErrorMessage = "El tipo de documento no puede exceder 50 caracteres")]
        public string? TipoDocumento { get; set; }

        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string? NroDocumento { get; set; }

        [StringLength(50, ErrorMessage = "El número de IIBB no puede exceder 50 caracteres")]
        public string? NroIIBB { get; set; }

        [StringLength(50, ErrorMessage = "La categoría IVA no puede exceder 50 caracteres")]
        public string? CategoriaIva { get; set; }

        [StringLength(50, ErrorMessage = "La condición de pago no puede exceder 50 caracteres")]
        public string? CondicionPago { get; set; }

        [Range(0, 100, ErrorMessage = "El descuento debe estar entre 0 y 100")]
        public decimal Descuento { get; set; }

        public string? Observaciones { get; set; }

        [StringLength(2, ErrorMessage = "El tipo de documento ARCA no puede exceder 2 caracteres")]
        public string? TipoDocArca { get; set; }
    }
}
