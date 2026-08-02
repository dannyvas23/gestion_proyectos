using GestionProyectos.Domain.Entidades;

namespace Domain.Puertos
{
    /// <summary>
    /// Puerto para operaciones de persistencia de columnas.
    /// </summary>
    public interface IColumnaRepositorio
    {
        Task<Columna?> ObtenerPorId(Guid id);
        Task<List<Columna>> ObtenerColumnasPorProyecto(Guid proyectoId);
        Task<Columna> Crear(Columna columna);
        Task Actualizar(Columna columna);
        Task<bool> TieneTareas(Guid columnaId);
        Task ActualizarOrden(List<Columna> columnas);
        Task<int> ObtenerMaximoOrden(Guid proyectoId);
        Task<List<Columna>> ObtenerPorProyecto(Guid proyectoId);
    }
}
