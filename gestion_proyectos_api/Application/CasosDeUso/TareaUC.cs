using Application.Excepciones;
using Domain.Puertos;
using Application.DTOs;
using Domain.Entidades;
using Domain.Enums;
using Domain.Puertos;

namespace Application.CasosDeUso;

/// <summary>
/// Casos de uso de tareas: CRUD
/// </summary>
public class TareaUC
{
    private readonly ITareaRepositorio _tareaRepo;
    private readonly IColumnaRepositorio _columnaRepo;

    private const double GAP_INICIAL = 1000.0;
    private const double GAP_MINIMO = 1.0;

    public TareaUC(ITareaRepositorio tareaRepo, IColumnaRepositorio columnaRepo)
    {
        _tareaRepo = tareaRepo;
        _columnaRepo = columnaRepo;
    }

    public async Task<List<TareaDto>> ObtenerPorProyecto(
        Guid proyectoId, Guid? responsableId = null, Prioridad? prioridad = null, string? busqueda = null)
    {
        var tareas = await _tareaRepo.ObtenerPorProyecto(proyectoId, responsableId, prioridad, busqueda);
        return tareas.Select(MapearEntidadADto).ToList();
    }

    public async Task<TareaDto> Crear(CrearTareaPeticion peticion)
    {
        _ = await _columnaRepo.ObtenerPorId(peticion.ColumnaId)
            ?? throw new NoEncontradoExcepcion("Columna", peticion.ColumnaId);

        var maxOrden = await _tareaRepo.ObtenerMaximoOrden(peticion.ColumnaId);

        var tarea = new Tarea
        {
            Id = Guid.NewGuid(),
            Titulo = peticion.Titulo,
            Descripcion = peticion.Descripcion,
            Prioridad = peticion.Prioridad,
            Orden = maxOrden + GAP_INICIAL,
            ColumnaId = peticion.ColumnaId,
            ResponsableId = peticion.ResponsableId,
            FechaCreacion = DateTime.UtcNow
        };

        await _tareaRepo.Crear(tarea);
        return MapearEntidadADto(tarea);
    }

    public async Task<TareaDto> Actualizar(Guid id, ActualizarTareaPeticion peticion)
    {
        var tarea = await _tareaRepo.ObtenerPorId(id)
            ?? throw new NoEncontradoExcepcion("Tarea", id);

        tarea.Titulo = peticion.Titulo;
        tarea.Descripcion = peticion.Descripcion;
        tarea.Prioridad = peticion.Prioridad;
        tarea.ResponsableId = peticion.ResponsableId;

        await _tareaRepo.Actualizar(tarea);
        return MapearEntidadADto(tarea);
    }

    public async Task Eliminar(Guid id)
    {
        _ = await _tareaRepo.ObtenerPorId(id)
            ?? throw new NoEncontradoExcepcion("Tarea", id);
        await _tareaRepo.Eliminar(id);
    }

    /// <summary>
    /// Mueve una tarea a una columna destino en una posición específica.
    /// Implementa la estrategia de gaps para calcular el nuevo orden.
    /// </summary>
    public async Task<TareaDto> Mover(MoverTareaPeticion peticion)
    {
        var tarea = await _tareaRepo.ObtenerPorId(peticion.TareaId)
            ?? throw new NoEncontradoExcepcion("Tarea", peticion.TareaId);

        _ = await _columnaRepo.ObtenerPorId(peticion.ColumnaDestinoId)
            ?? throw new NoEncontradoExcepcion("Columna", peticion.ColumnaDestinoId);

        // Obtener tareas de la columna destino ordenadas
        var tareasDestino = await _tareaRepo.ObtenerPorColumna(peticion.ColumnaDestinoId);
        var tareasOrdenadas = tareasDestino
            .Where(t => t.Id != tarea.Id) // excluir la tarea que se mueve
            .OrderBy(t => t.Orden)
            .ToList();

        // Calcular nueva posición
        var nuevaOrden = CalcularNuevaPosicion(
            tareasOrdenadas.Select(t => t.Orden).ToList(),
            peticion.NuevaPosicion);

        tarea.ColumnaId = peticion.ColumnaDestinoId;
        tarea.Orden = nuevaOrden;

        await _tareaRepo.Actualizar(tarea);

        // Si el gap es muy pequeño, renumerar toda la columna
        if (NecesitaRenumeracion(tareasOrdenadas, nuevaOrden, peticion.NuevaPosicion))
        {
            var todasLasTareas = await _tareaRepo.ObtenerPorColumna(peticion.ColumnaDestinoId);
            var renumeradas = todasLasTareas.OrderBy(t => t.Orden).ToList();
            for (int i = 0; i < renumeradas.Count; i++)
            {
                renumeradas[i].Orden = (i + 1) * GAP_INICIAL;
            }
            await _tareaRepo.ActualizarOrden(renumeradas);

            // Recargar la tarea con su nuevo orden
            tarea = await _tareaRepo.ObtenerPorId(peticion.TareaId);
        }

        return MapearEntidadADto(tarea!);
    }

    /// <summary>
    /// Calcula la nueva posición de una tarea al insertarla en un índice dado.
    /// 
    /// Estrategia:
    /// - Si la lista está vacía → posición = GAP_INICIAL (1000)
    /// - Si se inserta al inicio → posición = primer elemento / 2
    /// - Si se inserta al final → posición = último elemento + GAP_INICIAL
    /// - Si se inserta en medio → posición = promedio entre vecinos    /// 
    /// </summary>
    public static double CalcularNuevaPosicion(List<double> ordenesExistentes, int nuevaPosicion)
    {
        if (ordenesExistentes.Count == 0)
            return GAP_INICIAL;

        // Insertar al inicio
        if (nuevaPosicion <= 0)
            return ordenesExistentes[0] / 2.0;

        // Insertar al final
        if (nuevaPosicion >= ordenesExistentes.Count)
            return ordenesExistentes[^1] + GAP_INICIAL;

        // Insertar en medio: promedio entre el anterior y el actual en esa posición
        double anterior = ordenesExistentes[nuevaPosicion - 1];
        double siguiente = ordenesExistentes[nuevaPosicion];
        return (anterior + siguiente) / 2.0;
    }

    private bool NecesitaRenumeracion(List<Tarea> tareasOrdenadas, double nuevaOrden, int posicion)
    {
        if (tareasOrdenadas.Count == 0) return false;

        if (posicion > 0 && posicion <= tareasOrdenadas.Count)
        {
            double anterior = tareasOrdenadas[posicion - 1].Orden;
            if (Math.Abs(nuevaOrden - anterior) < GAP_MINIMO) return true;
        }

        if (posicion < tareasOrdenadas.Count)
        {
            double siguiente = tareasOrdenadas[posicion].Orden;
            if (Math.Abs(siguiente - nuevaOrden) < GAP_MINIMO) return true;
        }

        return nuevaOrden < GAP_MINIMO;
    }

    public static TareaDto MapearEntidadADto(Tarea t) => new()
    {
        Id = t.Id,
        Titulo = t.Titulo,
        Descripcion = t.Descripcion,
        Prioridad = t.Prioridad,
        Orden = t.Orden,
        FechaCreacion = t.FechaCreacion,
        ColumnaId = t.ColumnaId,
        ResponsableId = t.ResponsableId,
        ResponsableNombre = t.Responsable?.Nombre
    };
}
