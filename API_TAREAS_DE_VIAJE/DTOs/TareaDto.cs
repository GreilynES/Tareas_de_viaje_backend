namespace API_TAREAS_DE_VIAJE.Models.DTOs
{
    public class TareaDto
    {
        public int tarea_id { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public bool completada { get; set; }
        public int usuario_id { get; set; }
    }
}