using GestionProyectos.Domain.Entidades;

namespace GestionProyectos.Domain.Puertos;

/// <summary>
/// Puerto (contrato) para operaciones de persistencia de usuarios.
/// La implementación concreta vive en Infrastructure (adaptador).
/// </summary>
public interface IUsuarioRepositorio
{
    Task<List<Usuario>> ObtenerTodos();
    Task<Usuario> Crear(Usuario usuario);
    Task<bool> ExisteCorreo(string correo, Guid? excluirId = null);
}
