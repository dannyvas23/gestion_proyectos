using GestionProyectos.Application.CasosDeUso;
using GestionProyectos.Application.DTOs;
using GestionProyectos.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.API.Controllers;

/// <summary>
/// Controller de columnas del tablero.
/// </summary>
[ApiController]
[Route("api/columnas")]
public class ColumnasController : ControllerBase
{
    private readonly ColumnaUC _columnaUC;
    private readonly IServicioTablero _servicioTablero;

    public ColumnasController(ColumnaUC columnaUC, IServicioTablero servicioTablero)
    {
        _columnaUC = columnaUC;
        _servicioTablero = servicioTablero;
    }


    [HttpGet("proyecto/{proyectoId}")]
    public async Task<ActionResult<List<ColumnaDto>>> ObtenerPorProyecto(Guid proyectoId)
    {
        var columnas = await _columnaUC.ObtenerPorProyecto(proyectoId);
        return Ok(columnas);
    }

    [HttpPost]
    public async Task<ActionResult<ColumnaDto>> Crear([FromBody] CrearColumnaPeticion peticion)
    {
        var columna = await _columnaUC.Crear(peticion);
        await _servicioTablero.NotificarColumnaCreada(peticion.ProyectoId, columna);
        return CreatedAtAction(nameof(ObtenerPorProyecto), new { proyectoId = peticion.ProyectoId }, columna);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ColumnaDto>> Actualizar(Guid id, [FromBody] ActualizarColumnaPeticion peticion)
    {
        var columna = await _columnaUC.Actualizar(id, peticion);
        await _servicioTablero.NotificarColumnaActualizada(columna.ProyectoId, columna);
        return Ok(columna);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(Guid id)
    {
        // REGLA DE NEGOCIO: si la columna tiene tareas, no se puede eliminar.
        await _columnaUC.Eliminar(id);
        return NoContent();
    }

    [HttpPut("reordenar")]
    public async Task<ActionResult<List<ColumnaDto>>> Reordenar([FromBody] ReordenarColumnasPeticion peticion)
    {
        var columnas = await _columnaUC.Reordenar(peticion);
        await _servicioTablero.NotificarColumnasReordenadas(peticion.ProyectoId, columnas);
        return Ok(columnas);
    }

}
