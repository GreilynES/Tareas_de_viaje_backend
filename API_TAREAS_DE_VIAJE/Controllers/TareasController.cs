using API_TAREAS_DE_VIAJE.Models.DTOs;
using API_TAREAS_DE_VIAJE.Services.Tareas;
using Microsoft.AspNetCore.Mvc;

namespace API_TAREAS_DE_VIAJE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _service;
        private readonly ILogger<TareasController> _logger;

        public TareasController(ITareaService service, ILogger<TareasController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene el historial de tareas completadas
        /// </summary>
        [HttpGet("historial")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHistorial()
        {
            try
            {
                var historial = await _service.GetHistorialTareasAsync();
                return Ok(new
                {
                    success = true,
                    data = historial,
                    count = historial.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial de tareas");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener historial de tareas",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Obtiene todas las tareas con sus usuarios asignados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTareasUsuario()
        {
            try
            {
                var tareas = await _service.GetTareasUsuarioAsync();
                return Ok(new
                {
                    success = true,
                    data = tareas,
                    count = tareas.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener tareas");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al obtener tareas",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Crea una nueva tarea
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateTareaDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Datos inválidos",
                        errors = ModelState
                    });
                }

                var tareaId = await _service.InsertTareaAsync(dto);

                return CreatedAtAction(
                    nameof(Create),
                    new { id = tareaId },
                    new
                    {
                        success = true,
                        message = "Tarea creada exitosamente",
                        tarea_id = tareaId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear tarea");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al crear tarea",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Marca una tarea como completada
        /// </summary>
        [HttpPatch("{id}/completar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            try
            {
                var result = await _service.MarcarCompletadaAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Tarea con ID {id} no encontrada o ya está completada"
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Tarea marcada como completada exitosamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al marcar tarea como completada");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al marcar tarea como completada",
                    error = ex.Message
                });
            }
        }
    }
}