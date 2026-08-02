using Application.Excepciones;
using Domain.Puertos;
using GestionProyectos.Application.DTOs;
using GestionProyectos.Domain.Entidades;

namespace GestionProyectos.Application.CasosDeUso;

/// <summary>
/// Casos de uso de columnas.
/// </summary>
public class ColumnaUC
{
    private readonly IColumnaRepositorio _columnaRepo;

    public ColumnaUC(IColumnaRepositorio columnaRepo)
    {
        _columnaRepo = columnaRepo;
    }

    public async Task<List<ColumnaDto>> ObtenerColumnasPorProyecto(Guid proyectoId)
    {
        var columnas = await _columnaRepo.ObtenerColumnasPorProyecto(proyectoId);
        return columnas.Select(MapearEntidadADto).ToList();
    }

    public async Task<ColumnaDto> Crear(CrearColumnaPeticion peticion)
    {
        var maxOrden = await _columnaRepo.ObtenerMaximoOrden(peticion.ProyectoId);

        var columna = new Columna
        {
            Id = Guid.NewGuid(),
            Nombre = peticion.Nombre,
            Orden = maxOrden + 1,
            ProyectoId = peticion.ProyectoId,
            Activa = true
        };

        await _columnaRepo.Crear(columna);
        return MapearEntidadADto(columna);
    }

    public async Task<ColumnaDto> Actualizar(Guid id, ActualizarColumnaPeticion peticion)
    {
        var columna = await _columnaRepo.ObtenerPorId(id)
            ?? throw new NoEncontradoExcepcion("Columna", id);

        columna.Nombre = peticion.Nombre;
        await _columnaRepo.Actualizar(columna);
        return MapearEntidadADto(columna);
    }

    /// <summary>
    /// Elimina (desactiva) una columna.
    /// REGLA DE NEGOCIO: si la columna tiene tareas, no se puede desactivar.
    /// </summary>
    public async Task Eliminar(Guid id)
    {
        var columna = await _columnaRepo.ObtenerPorId(id)
            ?? throw new NoEncontradoExcepcion("Columna", id);

        if (await _columnaRepo.TieneTareas(id))
            throw new ReglaNegocioExcepcion(
                "No se puede eliminar una columna que contiene tareas. " +
                "Mueva o elimine las tareas primero.");

        columna.Activa = false;
        await _columnaRepo.Actualizar(columna);
    }

    /// <summary>
    /// Reordena las columnas del proyecto.
    /// Recibe la lista de IDs en el nuevo orden y asigna índices secuenciales.
    /// </summary>
    public async Task<List<ColumnaDto>> Reordenar(ReordenarColumnasPeticion peticion)
    {
        var columnas = await _columnaRepo.ObtenerColumnasPorProyecto(peticion.ProyectoId);
        var diccionario = columnas.ToDictionary(c => c.Id);

        for (int i = 0; i < peticion.ColumnasOrdenadas.Count; i++)
        {
            if (diccionario.TryGetValue(peticion.ColumnasOrdenadas[i], out var columna))
            {
                columna.Orden = i;
            }
        }

        await _columnaRepo.ActualizarOrden(columnas);
        return columnas.OrderBy(c => c.Orden).Select(MapearEntidadADto).ToList();
    }

    public async Task<List<ColumnaDto>> ObtenerPorProyecto(Guid proyectoId)
    {
        var columnas = await _columnaRepo.ObtenerPorProyecto(proyectoId);
        return columnas.Select(proyec =>MapearEntidadADto(proyec)).ToList();
    }

    private static ColumnaDto MapearEntidadADto(Columna c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        Orden = c.Orden,
        ProyectoId = c.ProyectoId,
        Activa = c.Activa,
        Tareas = c.Tareas?.OrderBy(t => t.Orden).Select(TareaUC.MapearEntidadADto).ToList() ?? new()
    };
}
