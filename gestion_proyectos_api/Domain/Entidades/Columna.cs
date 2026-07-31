namespace GestionProyectos.Domain.Entidades;

/// <summary>
/// Representa una columna del tablero kanban de un proyecto. (ej: Por Hacer, En Progreso, Hecho).
/// El campo Orden determina la posición visual de izquierda a derecha.
/// </summary>
public class Columna
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public bool Activa { get; set; } = true;

    // Clave foránea
    public Guid ProyectoId { get; set; }
    public Proyecto Proyecto { get; set; } = null!;

    // Navegación: tareas dentro de esta columna
    public ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
