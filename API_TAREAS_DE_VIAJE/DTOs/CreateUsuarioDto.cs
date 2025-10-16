using System.ComponentModel.DataAnnotations;

namespace API_TAREAS_DE_VIAJE.Models.DTOs
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(20, ErrorMessage = "El nombre no puede exceder 20 caracteres")]
        public string nombre { get; set; } = string.Empty;
    }
}