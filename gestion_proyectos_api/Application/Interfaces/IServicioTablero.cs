

using GestionProyectos.Application.DTOs;

namespace GestionProyectos.Application.Interfaces;

public interface IServicioTablero
{
    Task NotificarTareaMovida(Guid proyectoId, TareaDto tarea);
    Task NotificarTareaCreada(Guid proyectoId, TareaDto tarea);
    Task NotificarTareaActualizada(Guid proyectoId, TareaDto tarea);
    Task NotificarTareaEliminada(Guid proyectoId, Guid tareaId);
    Task NotificarColumnaCreada(Guid proyectoId, ColumnaDto columna);
    Task NotificarColumnaActualizada(Guid proyectoId, ColumnaDto columna);
    Task NotificarColumnaEliminada(Guid proyectoId, Guid columnaId);
    Task NotificarColumnasReordenadas(Guid proyectoId, List<ColumnaDto> columnas);
}
