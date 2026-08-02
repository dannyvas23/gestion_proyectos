using GestionProyectos.Domain.Entidades;
using GestionProyectos.Domain.Enums;

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
        Task<List<Tarea>> ObtenerPorProyecto(Guid proyectoId, Guid? responsableId = null, Prioridad? prioridad = null, string? busqueda = null);
        Task<double> ObtenerMaximoOrden(Guid columnaId);

        Task<List<Tarea>> ObtenerPorColumna(Guid columnaId);
        Task ActualizarOrden(List<Tarea> tareas);
    }
}