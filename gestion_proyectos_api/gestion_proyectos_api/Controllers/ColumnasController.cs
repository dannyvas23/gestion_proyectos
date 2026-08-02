using GestionProyectos.Application.CasosDeUso;
using GestionProyectos.Application.DTOs;
using GestionProyectos.Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
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

    public ColumnasController(ColumnaUC columnaUC)
    {
        _columnaUC = columnaUC;
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
        return CreatedAtAction(nameof(ObtenerPorProyecto), new { proyectoId = peticion.ProyectoId }, columna);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ColumnaDto>> Actualizar(Guid id, [FromBody] ActualizarColumnaPeticion peticion)
    {
        var columna = await _columnaUC.Actualizar(id, peticion);
        //todo notificar a los usuarios que la columna ha sido actualizada
        return Ok(columna);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(Guid id)
    {
        // REGLA DE NEGOCIO: si la columna tiene tareas, no se puede eliminar.
        await _columnaUC.Eliminar(id);
        return NoContent();
    }


}
