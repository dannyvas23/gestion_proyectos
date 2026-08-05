using Application.CasosDeUso;
using Application.Comun;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gestion_proyectos_api.Controllers
{

    /// <summary>
    /// Controller de proyecto con CRUD completo.
    /// - Administrador puede crear/editar/eliminar. 
    /// - Miembro solo puede ver.
    /// </summary>
    [ApiController]
    [Route("api/proyectos")]    
    public class ProyectoController : ControllerBase
    {
        private readonly ProyectoUC _proyectoUC;

        public ProyectoController(ProyectoUC proyectoUC)
        {
            _proyectoUC = proyectoUC;
        }

        [HttpGet]public async Task<ActionResult<RespuestaPaginada<ProyectoDto>>> ListarProyectos(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanio = 10,
        [FromQuery] string? nombre = null)
        {
            var resultado = await _proyectoUC.ListarProyectos(pagina, tamanio, nombre);
            return Ok(resultado);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProyectoDto>> ObtenerPorId(Guid id)
        {
            var proyecto = await _proyectoUC.ObtenerPorId(id);
            return Ok(proyecto);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<ProyectoDto>> Crear([FromBody] CrearProyectoPeticion peticion)
        {
            var proyecto = await _proyectoUC.Crear(peticion);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = proyecto.Id }, proyecto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProyectoDto>> Actualizar(Guid id, [FromBody] ActualizarProyectoPeticion peticion)
        {
            var proyecto = await _proyectoUC.Actualizar(id, peticion);
            return Ok(proyecto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(Guid id)
        {
            await _proyectoUC.Eliminar(id);
            return NoContent();
        }
    }
}
