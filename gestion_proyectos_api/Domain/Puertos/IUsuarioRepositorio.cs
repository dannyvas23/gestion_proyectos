using Domain.Entidades;

namespace Domain.Puertos;

/// <summary>
/// Puerto (contrato) para operaciones de persistencia de usuarios.
/// </summary>
public interface IUsuarioRepositorio
{
    Task<List<Usuario>> ObtenerTodos();
    Task<Usuario> Crear(Usuario usuario);
    Task<bool> ExisteCorreo(string correo, Guid? excluirId = null);
    Task<Usuario?> ObtenerPorCorreo(string correo);
}
