using GestionProyectos.Domain.Entidades;

namespace GestionProyectos.Domain.Puertos
{ 
    /// <summary>
    /// Puerto para operaciones de persistencia de tareas.
    /// </summary>
    public interface ITareaRepositorio
    {
        Task<Tarea?> ObtenerPorId(Guid id);
        Task<Tarea> Crear(Tarea tarea);
        Task Actualizar(Tarea tarea);
        Task Eliminar(Guid id);
    }
}