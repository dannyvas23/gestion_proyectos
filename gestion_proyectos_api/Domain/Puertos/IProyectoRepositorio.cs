using Domain.Entidades;

namespace Domain.Puertos
{ 
    /// <summary>
    /// Puerto para operaciones de persistencia de proyectos.
    /// </summary>
    public interface IProyectoRepositorio
    {
        Task<Proyecto?> ObtenerPorId(Guid id);
        Task<(List<Proyecto> Items, int Total)> ListarProyectos(int pagina, int tamanio, string? filtroNombre = null);
        Task<Proyecto> Crear(Proyecto proyecto);
        Task Actualizar(Proyecto proyecto);
        Task Eliminar(Guid id);
    }

}