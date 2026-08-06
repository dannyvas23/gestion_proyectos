using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Servicios.ComunicacionContinua;

/// <summary>
/// Implementación del servicio de notificaciones en tiempo real.
/// Usa IHubContext para enviar mensajes desde fuera del Hub (desde los controllers).
/// - Clients.GroupExcept: envía a todos en el grupo EXCEPTO al usuario que originó el cambio
///   (evita que reciba su propio evento).
/// </summary>
public class ServicioTablero : IServicioTablero
{
    private readonly IHubContext<TableroHub> _hubContext;

    public ServicioTablero(IHubContext<TableroHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotificarTareaMovida(Guid proyectoId, TareaDto tarea)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("TareaMovida", tarea);
    }

    public async Task NotificarTareaCreada(Guid proyectoId, TareaDto tarea)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("TareaCreada", tarea);
    }

    public async Task NotificarTareaActualizada(Guid proyectoId, TareaDto tarea)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("TareaActualizada", tarea);
    }

    public async Task NotificarTareaEliminada(Guid proyectoId, Guid tareaId)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("TareaEliminada", tareaId);
    }

    public async Task NotificarColumnaCreada(Guid proyectoId, ColumnaDto columna)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("ColumnaCreada", columna);
    }

    public async Task NotificarColumnaActualizada(Guid proyectoId, ColumnaDto columna)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("ColumnaActualizada", columna);
    }

    public async Task NotificarColumnaEliminada(Guid proyectoId, Guid columnaId)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("ColumnaEliminada", columnaId);
    }

    public async Task NotificarColumnasReordenadas(Guid proyectoId, List<ColumnaDto> columnas)
    {
        await _hubContext.Clients.Group($"tablero-{proyectoId}")
            .SendAsync("ColumnasReordenadas", columnas);
    }
}
