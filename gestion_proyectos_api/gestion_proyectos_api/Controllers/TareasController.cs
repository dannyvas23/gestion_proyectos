using Application.CasosDeUso;
using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

/// <summary>
/// Controller de tareas del tablero Kanban.
/// Incluye CRUD, movimiento entre columnas y filtros.
/// </summary>
[ApiController]
[Route("api/tareas")]
public class TareasController : ControllerBase
{
    private readonly TareaUC _tareaUC;
    private readonly IServicioTablero _servicioTablero;

    public TareasController(TareaUC tareaUC, IServicioTablero servicioTablero)
    {
        _tareaUC = tareaUC;
        _servicioTablero = servicioTablero;
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
            await _servicioTablero.NotificarTareaCreada(pid, tarea);
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
            await _servicioTablero.NotificarTareaActualizada(pid, tarea);
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
            await _servicioTablero.NotificarTareaEliminada(pid, id);
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
            await _servicioTablero.NotificarTareaMovida(pid, tarea);
        }

        return Ok(tarea);
    }
}
