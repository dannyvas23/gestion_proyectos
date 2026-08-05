using Domain.Entidades;
using Domain.Puertos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositorios;

/// <summary>
/// Adaptador: implementación de IProyectoRepositorio.
/// </summary>
public class ProyectoRepositorio : IProyectoRepositorio
{
    private readonly AppDbContext _context;

    public ProyectoRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Proyecto?> ObtenerPorId(Guid id)
        => await _context.Proyectos
            .Include(p => p.Columnas.Where(c => c.Activa).OrderBy(c => c.Orden))
                .ThenInclude(c => c.Tareas.OrderBy(t => t.Orden))
                    .ThenInclude(t => t.Responsable)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<(List<Proyecto> Items, int Total)> ListarProyectos(
        int pagina, int tamanio, string? filtroNombre = null)
    {
        var query = _context.Proyectos.Where(p => p.Activo);

        if (!string.IsNullOrWhiteSpace(filtroNombre))
        {
            query = query.Where(p => p.Nombre.ToLower().Contains(filtroNombre.ToLower()));
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.FechaInicio)
            .Skip((pagina - 1) * tamanio)
            .Take(tamanio)
            .ToListAsync();

        return (items, total);
    }

    public async Task<Proyecto> Crear(Proyecto proyecto)
    {
        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task Actualizar(Proyecto proyecto)
    {
        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();
    }

    public async Task Eliminar(Guid id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto != null)
        {
            proyecto.Activo = false;
            await _context.SaveChangesAsync();
        }
    }
}
