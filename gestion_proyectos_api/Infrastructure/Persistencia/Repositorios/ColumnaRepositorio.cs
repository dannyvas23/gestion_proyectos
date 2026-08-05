using Domain.Puertos;
using Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositorios;


/// <summary>
/// Adaptador: implementación de IColumnaRepositorio.
/// </summary>
public class ColumnaRepositorio : IColumnaRepositorio
{
    private readonly AppDbContext _context;

    public ColumnaRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Columna?> ObtenerPorId(Guid id)
        => await _context.Columnas
            .Include(c => c.Tareas.OrderBy(t => t.Orden))
                .ThenInclude(t => t.Responsable)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<List<Columna>> ObtenerColumnasPorProyecto(Guid proyectoId)
        => await _context.Columnas
            .Where(c => c.ProyectoId == proyectoId && c.Activa)
            .Include(c => c.Tareas.OrderBy(t => t.Orden))
                .ThenInclude(t => t.Responsable)
            .OrderBy(c => c.Orden)
            .ToListAsync();

    public async Task<Columna> Crear(Columna columna)
    {
        _context.Columnas.Add(columna);
        await _context.SaveChangesAsync();
        return columna;
    }

    public async Task Actualizar(Columna columna)
    {
        _context.Columnas.Update(columna);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TieneTareas(Guid columnaId)
        => await _context.Tareas.AnyAsync(t => t.ColumnaId == columnaId);

    public async Task ActualizarOrden(List<Columna> columnas)
    {
        _context.Columnas.UpdateRange(columnas);
        await _context.SaveChangesAsync();
    }

    public async Task<int> ObtenerMaximoOrden(Guid proyectoId)
    {
        var maxOrden = await _context.Columnas
            .Where(c => c.ProyectoId == proyectoId && c.Activa)
            .MaxAsync(c => (int?)c.Orden);
        return maxOrden ?? -1;
    }

    public async Task<List<Columna>> ObtenerPorProyecto(Guid proyectoId)
        => await _context.Columnas
            .Where(c => c.ProyectoId == proyectoId && c.Activa)
            .Include(c => c.Tareas.OrderBy(t => t.Orden))
                .ThenInclude(t => t.Responsable)
            .OrderBy(c => c.Orden)
            .ToListAsync();
}

    
