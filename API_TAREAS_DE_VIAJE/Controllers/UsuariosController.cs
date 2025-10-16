using API_TAREAS_DE_VIAJE.Models.DTOs;
using API_TAREAS_DE_VIAJE.Services.Usuarios;
using Microsoft.AspNetCore.Mvc;

namespace API_TAREAS_DE_VIAJE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService service, ILogger<UsuariosController> logger)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDto dto)
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

                var usuarioId = await _service.InsertUsuarioAsync(dto);

                return CreatedAtAction(
                    nameof(Create),
                    new { id = usuarioId },
                    new
                    {
                        success = true,
                        message = "Usuario creado exitosamente",
                        usuario_id = usuarioId
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error al crear usuario",
                    error = ex.Message
                });
            }
        }
    }
}