using GestionProyectos.Domain.Enums;

namespace GestionProyectos.Domain.Entidades;

/// <summary>
/// Representa un proyecto ágil con su tablero kanban.
/// Un proyecto contiene múltiples columnas que definen el flujo de trabajo.
/// </summary>
public class Proyecto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFinPrevista { get; set; }
    public EstadoProyecto Estado { get; set; } = EstadoProyecto.Activo;
    public bool Activo { get; set; } = true;

    // Navegación: columnas del tablero
    public ICollection<Columna> Columnas { get; set; } = new List<Columna>();
}
