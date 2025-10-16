using API_TAREAS_DE_VIAJE.Models.DTOs;
using API_TAREAS_DE_VIAJE.Services.Database;
using Microsoft.Data.SqlClient;
using System.Data;

namespace API_TAREAS_DE_VIAJE.Services.Tareas
{
    public class TareaService : ITareaService
    {
        private readonly DatabaseService _databaseService;
        private readonly ILogger<TareaService> _logger;

        public TareaService(DatabaseService databaseService, ILogger<TareaService> logger)
        {
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task<int> InsertTareaAsync(CreateTareaDto dto)
        {
            try
            {
                _logger.LogInformation("Insertando nueva tarea");

                using var connection = await _databaseService.GetOpenConnectionAsync();

                // Ejecutar el SP
                using var command = new SqlCommand("sp_agregar_tarea", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@_titulo", dto.titulo);
                command.Parameters.AddWithValue("@_descripcion", dto.descripcion ?? string.Empty);
                command.Parameters.AddWithValue("@_usuario_id", dto.usuario_id);

                await command.ExecuteNonQueryAsync();

                // Obtener el último ID insertado usando MAX (solución temporal)
                using var commandId = new SqlCommand("SELECT MAX(tarea_id) FROM Tarea WHERE usuario_id = @usuario_id", connection);
                commandId.Parameters.AddWithValue("@usuario_id", dto.usuario_id);

                var result = await commandId.ExecuteScalarAsync();

                return result != DBNull.Value && result != null
                    ? Convert.ToInt32(result)
                    : 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al insertar tarea");
                throw;
            }
        }

        public async Task<bool> MarcarCompletadaAsync(int tareaId)
        {
            try
            {
                _logger.LogInformation("Marcando tarea ID: {Id} como completada", tareaId);

                using var connection = await _databaseService.GetOpenConnectionAsync();
                using var command = new SqlCommand("sp_marcar_tarea_completada", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@_tarea_id", tareaId);

                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (SqlException ex) when (ex.Message.Contains("no existe"))
            {
                _logger.LogWarning("Tarea ID {Id} no encontrada", tareaId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar tarea como completada");
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetHistorialTareasAsync()
        {
            var historial = new List<object>();
            try
            {
                _logger.LogInformation("Obteniendo historial de tareas");

                using var connection = await _databaseService.GetOpenConnectionAsync();
                using var command = new SqlCommand("SELECT * FROM vw_historial_tareas", connection);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    historial.Add(new
                    {
                        IdHistorial = reader.GetInt32(reader.GetOrdinal("IdHistorial")),
                        IdTarea = reader.GetInt32(reader.GetOrdinal("IdTarea")),
                        Tarea = reader.GetString(reader.GetOrdinal("Tarea")),
                        Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion"))
                            ? ""
                            : reader.GetString(reader.GetOrdinal("Descripcion")),
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        FechaCompletado = reader.GetDateTime(reader.GetOrdinal("FechaCompletado")).ToString("dd 'de' MMMM 'de' yyyy, hh:mm tt", new System.Globalization.CultureInfo("es-ES"))
                    });
                }

                _logger.LogInformation("Se obtuvieron {Count} registros del historial", historial.Count);
                return historial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de tareas");
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetTareasUsuarioAsync()
        {
            var tareas = new List<object>();
            try
            {
                _logger.LogInformation("Obteniendo tareas de usuarios");

                using var connection = await _databaseService.GetOpenConnectionAsync();
                using var command = new SqlCommand("SELECT * FROM vw_tareas_usuario", connection);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tareas.Add(new
                    {
                        Tarea = reader.GetString(reader.GetOrdinal("Tarea")),
                        UsuarioAsignado = reader.GetString(reader.GetOrdinal("Usuario asignado")),
                        Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                        Completada = reader.GetBoolean(reader.GetOrdinal("Completada"))
                    });
                }

                return tareas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas de usuarios");
                throw;
            }
        }
    }
}