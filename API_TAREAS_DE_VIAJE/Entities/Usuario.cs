using System.Threading;

namespace API_TAREAS_DE_VIAJE.Entities
{
    public class Usuario
    {
        public int usuario_id { get; set; }
        public string nombre { get; set; } = string.Empty;

        // Relación con Tareas
        public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
    }
}
