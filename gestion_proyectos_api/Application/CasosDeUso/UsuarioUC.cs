using Application.DTOs;
using Application.Excepciones;
using GestionProyectos.Domain.Entidades;
using GestionProyectos.Domain.Puertos;

namespace GestionProyectos.Application.CasosDeUso;

/// <summary>
/// Casos de uso de usuarios (solo se creo principales).
/// </summary>
public class UsuarioUC
{
    private readonly IUsuarioRepositorio _usuarioRepo;

    public UsuarioUC(IUsuarioRepositorio usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }

    public async Task<List<UsuarioDto>> ObtenerTodos()
    {
        var usuarios = await _usuarioRepo.ObtenerTodos();
        return usuarios.Select(usu => MapearEntidadADto(usu)).ToList();
    }
    

    public async Task<UsuarioDto> Crear(CrearUsuarioPeticion peticion)
    {
        if (await _usuarioRepo.ExisteCorreo(peticion.CorreoElectronico))
            throw new ReglaNegocioExcepcion("El correo electrónico ya está registrado.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = peticion.Nombre,
            CorreoElectronico = peticion.CorreoElectronico,
            PasswordHash = "TODO IMPPLEMENTAR HASH", //TODO
            Rol = peticion.Rol,
            Activo = true
        };

        await _usuarioRepo.Crear(usuario);
        return MapearEntidadADto(usuario);
    }

   
    private static UsuarioDto MapearEntidadADto(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        CorreoElectronico = u.CorreoElectronico,
        Rol = u.Rol,
        Activo = u.Activo
    };
}
