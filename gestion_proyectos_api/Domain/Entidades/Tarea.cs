using GestionProyectos.Domain.Enums;

namespace GestionProyectos.Domain.Entidades;

/// <summary>
/// Representa una tarea dentro de una columna del tablero kanban.
/// El campo Orden determina la posición vertical dentro de la columna (de arriba a abajo).
/// </summary>
public class Tarea
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Prioridad Prioridad { get; set; } = Prioridad.Media;
    public double Orden { get; set; }
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Clave foránea: columna a la que pertenece
    public Guid ColumnaId { get; set; }
    public Columna Columna { get; set; } = null!;

    // Clave foránea: usuario responsable (puede ser null si no se ha asignado)
    public Guid? ResponsableId { get; set; }
    public Usuario? Responsable { get; set; }
}
