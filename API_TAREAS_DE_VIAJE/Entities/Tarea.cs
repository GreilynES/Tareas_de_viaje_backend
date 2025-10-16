namespace API_TAREAS_DE_VIAJE.Entities
{
    public class Tarea
    {
        public int tareaId { get; set; }
        public string titulo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public bool completada { get; set; }

        // Llave foránea
        public int usuario_id { get; set; }

        // Propiedad de navegación
        public Usuario Usuario { get; set; } = null!;
    }
}
