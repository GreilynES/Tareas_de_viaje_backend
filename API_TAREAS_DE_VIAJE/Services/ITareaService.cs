using API_TAREAS_DE_VIAJE.Models.DTOs;

namespace API_TAREAS_DE_VIAJE.Services.Tareas
{
    public interface ITareaService
    {
        Task<int> InsertTareaAsync(CreateTareaDto dto);
        Task<bool> MarcarCompletadaAsync(int tareaId);
        Task<IEnumerable<object>> GetHistorialTareasAsync();
        Task<IEnumerable<object>> GetTareasUsuarioAsync();
    }
}