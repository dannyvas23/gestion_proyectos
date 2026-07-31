using GestionProyectos.Domain.Enums;

namespace GestionProyectos.Domain.Entidades;

/// <summary>
/// Representa un usuario del sistema.
/// </summary>
public class Usuario
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string CorreoElectronico { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;

    // Para Navegación: tareas asignadas a este usuario
    public ICollection<Tarea> TareasAsignadas { get; set; } = new List<Tarea>();
}
