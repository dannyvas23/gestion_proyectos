using Domain.Entidades;
using Domain.Enums;
using Domain.Puertos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositorios;


/// <summary>
/// Adaptador: implementación de ITareaRepositorio.
/// </summary>
public class TareaRepositorio : ITareaRepositorio
{
    private readonly AppDbContext _context;

    public TareaRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Tarea?> ObtenerPorId(Guid id)
        => await _context.Tareas
            .Include(t => t.Responsable)
            .Include(t => t.Columna)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<Tarea> Crear(Tarea tarea)
    {
        _context.Tareas.Add(tarea);
        await _context.SaveChangesAsync();
        return tarea;
    }

    public async Task Actualizar(Tarea tarea)
    {
        _context.Tareas.Update(tarea);
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Guid id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea != null)
        {
            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Columna>> ObtenerPorProyecto(Guid proyectoId)
    => await _context.Columnas
        .Where(c => c.ProyectoId == proyectoId && c.Activa)
        .Include(c => c.Tareas.OrderBy(t => t.Orden))
            .ThenInclude(t => t.Responsable)
        .OrderBy(c => c.Orden)
        .ToListAsync();


    public async Task<List<Tarea>> ObtenerPorProyecto(
        Guid proyectoId, Guid? responsableId = null, Prioridad? prioridad = null, string? busqueda = null)
    {
        var query = _context.Tareas
            .Include(t => t.Responsable)
            .Include(t => t.Columna)
            .Where(t => t.Columna.ProyectoId == proyectoId && t.Columna.Activa);

        if (responsableId.HasValue)
            query = query.Where(t => t.ResponsableId == responsableId.Value);

        if (prioridad.HasValue)
            query = query.Where(t => t.Prioridad == prioridad.Value);

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(t =>
                t.Titulo.ToLower().Contains(busqueda.ToLower()) ||
                t.Descripcion.ToLower().Contains(busqueda.ToLower()));

        return await query.OrderBy(t => t.Orden).ToListAsync();
    }

    public async Task<double> ObtenerMaximoOrden(Guid columnaId)
    {
        var maxOrden = await _context.Tareas
            .Where(t => t.ColumnaId == columnaId)
            .MaxAsync(t => (double?)t.Orden);
        return maxOrden ?? 0;
    }

    public async Task<List<Tarea>> ObtenerPorColumna(Guid columnaId)
    => await _context.Tareas
        .Where(t => t.ColumnaId == columnaId)
        .Include(t => t.Responsable)
        .OrderBy(t => t.Orden)
        .ToListAsync();

    public async Task ActualizarOrden(List<Tarea> tareas)
    {
        _context.Tareas.UpdateRange(tareas);
        await _context.SaveChangesAsync();
    }
}
