using API_TAREAS_DE_VIAJE.Models.DTOs;

namespace API_TAREAS_DE_VIAJE.Services.Usuarios
{
    public interface IUsuarioService
    {
        Task<int> InsertUsuarioAsync(CreateUsuarioDto dto);
    }
}