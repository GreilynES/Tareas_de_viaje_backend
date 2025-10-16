using System.ComponentModel.DataAnnotations;

namespace API_TAREAS_DE_VIAJE.Models.DTOs
{
    public class CreateTareaDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(20, ErrorMessage = "El título no puede exceder 20 caracteres")]
        public string titulo { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "La descripción no puede exceder 50 caracteres")]
        public string descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario_id es requerido")]
        public int usuario_id { get; set; }
    }
}