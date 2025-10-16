using API_TAREAS_DE_VIAJE.Models.DTOs;
using API_TAREAS_DE_VIAJE.Services.Database;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_TAREAS_DE_VIAJE.Services.Usuarios
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(DatabaseService databaseService, ILogger<UsuarioService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<int> InsertUsuarioAsync(CreateUsuarioDto dto)
        {
            try
            {
                _logger.LogInformation("Insertando nuevo usuario");

                using var connection = await _databaseService.GetOpenConnectionAsync();

                // Ejecutar el SP
                using var command = new SqlCommand("sp_agregar_usuario", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@_nombre", dto.nombre);
                await command.ExecuteNonQueryAsync();

                // Obtener el último ID insertado en una consulta separada pero en la misma conexión
                using var commandId = new SqlCommand("SELECT MAX(usuario_id) FROM Usuario", connection);
                var result = await commandId.ExecuteScalarAsync();

                return result != DBNull.Value && result != null
                    ? Convert.ToInt32(result)
                    : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar usuario");
                throw;
            }
        }
    }
}