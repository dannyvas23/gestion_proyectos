using GestionProyectos.Domain.Enums;

namespace GestionProyectos.Application.DTOs;

public class TareaDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Prioridad Prioridad { get; set; }
    public double Orden { get; set; }
    public DateTime FechaCreacion { get; set; }
    public Guid ColumnaId { get; set; }
    public Guid? ResponsableId { get; set; }
    public string? ResponsableNombre { get; set; }
}

public class CrearTareaPeticion
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Prioridad Prioridad { get; set; } = Prioridad.Media;
    public Guid ColumnaId { get; set; }
    public Guid? ResponsableId { get; set; }
}

public class ActualizarTareaPeticion
{
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Prioridad Prioridad { get; set; }
    public Guid? ResponsableId { get; set; }
}

/// <summary>
/// Petición para mover/reordenar una tarea.
/// Se puede mover entre columnas y cambiar posición.
/// </summary>
public class MoverTareaPeticion
{
    public Guid TareaId { get; set; }
    public Guid ColumnaDestinoId { get; set; }
    public int NuevaPosicion { get; set; }
}
