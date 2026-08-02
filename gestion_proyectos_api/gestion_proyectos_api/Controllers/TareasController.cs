using System.Security.Claims;
using GestionProyectos.Application.CasosDeUso;
using GestionProyectos.Application.DTOs;
using GestionProyectos.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.API.Controllers;

/// <summary>
/// Controller de tareas del tablero Kanban.
/// Incluye CRUD, movimiento entre columnas y filtros.
/// </summary>
[ApiController]
[Route("api/tareas")]
public class TareasController : ControllerBase
{
    private readonly TareaUC _tareaUC;

    public TareasController(TareaUC tareaUC)
    {
        _tareaUC = tareaUC;
    }

    [HttpGet("proyecto/{proyectoId}")]
    public async Task<ActionResult<List<TareaDto>>> ObtenerPorProyecto(
        Guid proyectoId,
        [FromQuery] Guid? responsableId = null,
        [FromQuery] Prioridad? prioridad = null,
        [FromQuery] string? busqueda = null)
    {
        var tareas = await _tareaUC.ObtenerPorProyecto(
            proyectoId, responsableId, prioridad, busqueda);
        return Ok(tareas);
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Crear([FromBody] CrearTareaPeticion peticion)
    {
        var tarea = await _tareaUC.Crear(peticion);

        // Obtener el proyectoId desde la columna para notificar
        var columnaId = peticion.ColumnaId;
        // Por simplicidad, el frontend envía el proyectoId en el header
        var proyectoId = Request.Headers["X-Proyecto-Id"].FirstOrDefault();
        if (Guid.TryParse(proyectoId, out var pid))
        {
            // TODO: Notificar a los clientes conectados al tablero del proyecto
        }

        return CreatedAtAction(nameof(ObtenerPorProyecto), new { proyectoId }, tarea);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TareaDto>> Actualizar(Guid id, [FromBody] ActualizarTareaPeticion peticion)
    {
        var tarea = await _tareaUC.Actualizar(id, peticion);

        var proyectoId = Request.Headers["X-Proyecto-Id"].FirstOrDefault();
        if (Guid.TryParse(proyectoId, out var pid))
        {
            // TODO: Notificar a los clientes conectados al tablero del proyecto
        }

        return Ok(tarea);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Eliminar(Guid id)
    {
        var proyectoId = Request.Headers["X-Proyecto-Id"].FirstOrDefault();

        await _tareaUC.Eliminar(id);

        if (Guid.TryParse(proyectoId, out var pid))
        {
            // TODO: Notificar a los clientes conectados al tablero del proyecto
        }

        return NoContent();
    }

    [HttpPut("mover")]
    public async Task<ActionResult<TareaDto>> Mover([FromBody] MoverTareaPeticion peticion)
    {
        var tarea = await _tareaUC.Mover(peticion);

        var proyectoId = Request.Headers["X-Proyecto-Id"].FirstOrDefault();
        if (Guid.TryParse(proyectoId, out var pid))
        {
            // TODO: Notificar a los clientes conectados al tablero del proyecto
        }

        return Ok(tarea);
    }
}
