using Application.Comun;
using Application.DTOs;
using Application.Excepciones;
using Domain.Entidades;
using Domain.Enums;
using Domain.Puertos;

namespace Application.CasosDeUso
{
    /// <summary>
    /// Casos de uso de proyectos: CRUD con paginación y filtro por nombre.
    /// </summary>
    public class ProyectoUC
    {
        private readonly IProyectoRepositorio _proyectoRepo;

        public ProyectoUC(IProyectoRepositorio proyectoRepo)
        {
            _proyectoRepo = proyectoRepo;
        }

        public async Task<ProyectoDto> ObtenerPorId(Guid id)
        {
            var proyecto = await _proyectoRepo.ObtenerPorId(id)
                ?? throw new NoEncontradoExcepcion("Proyecto", id);
            return MapearEntidadADto(proyecto);
        }

        public async Task<RespuestaPaginada<ProyectoDto>> ListarProyectos(int pagina, int tamanio, string? filtroNombre)
        {
            var (items, total) = await _proyectoRepo.ListarProyectos(pagina, tamanio, filtroNombre);

            return new RespuestaPaginada<ProyectoDto>
            {
                Items = items.Select(MapearEntidadADto).ToList(),
                Total = total,
                Pagina = pagina,
                Tamanio = tamanio
            };
        }

    
        public async Task<ProyectoDto> Crear(CrearProyectoPeticion peticion)
        {
            var proyecto = new Proyecto
            {
                Id = Guid.NewGuid(),
                Nombre = peticion.Nombre,
                Descripcion = peticion.Descripcion,
                FechaInicio = peticion.FechaInicio,
                FechaFinPrevista = peticion.FechaFinPrevista,
                Estado = EstadoProyecto.Activo,
                Activo = true
            };

            await _proyectoRepo.Crear(proyecto);
            return MapearEntidadADto(proyecto);
        }

        public async Task<ProyectoDto> Actualizar(Guid id, ActualizarProyectoPeticion peticion)
        {
            var proyecto = await _proyectoRepo.ObtenerPorId(id)
                ?? throw new NoEncontradoExcepcion("Proyecto", id);

            proyecto.Nombre = peticion.Nombre;
            proyecto.Descripcion = peticion.Descripcion;
            proyecto.FechaInicio = peticion.FechaInicio;
            proyecto.FechaFinPrevista = peticion.FechaFinPrevista;
            proyecto.Estado = peticion.Estado;

            await _proyectoRepo.Actualizar(proyecto);
            return MapearEntidadADto(proyecto);
        }

        public async Task Eliminar(Guid id)
        {
            _ = await _proyectoRepo.ObtenerPorId(id)
                ?? throw new NoEncontradoExcepcion("Proyecto", id);
            await _proyectoRepo.Eliminar(id);
        }

        private static ProyectoDto MapearEntidadADto(Proyecto p) => new()
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            FechaInicio = p.FechaInicio,
            FechaFinPrevista = p.FechaFinPrevista,
            Estado = p.Estado,
            Activo = p.Activo
        };

    }
}
