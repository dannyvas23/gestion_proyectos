using Domain.Entidades;
using Domain.Puertos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistencia.Repositorios;

/// <summary>
/// Adaptador: implementación de IUsuarioRepositorio.
/// </summary>
public class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AppDbContext _context;

    public UsuarioRepositorio(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Usuario>> ObtenerTodos()
        => await _context.Usuarios.OrderBy(u => u.Nombre).ToListAsync();

    public async Task<Usuario> Crear(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> ExisteCorreo(string correo, Guid? excluirId = null)
    {
        var query = _context.Usuarios
            .Where(u => u.CorreoElectronico.ToLower() == correo.ToLower());

        if (excluirId.HasValue)
            query = query.Where(u => u.Id != excluirId.Value);

        return await query.AnyAsync();
    }

    public async Task<Usuario?> ObtenerPorCorreo(string correo)
       => await _context.Usuarios
           .FirstOrDefaultAsync(u => u.CorreoElectronico.ToLower() == correo.ToLower());

}
