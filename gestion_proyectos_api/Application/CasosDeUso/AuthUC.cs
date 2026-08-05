using Application.DTOs;
using Application.Excepciones;
using Application.Interfaces;
using Domain.Entidades;
using Domain.Puertos;

namespace Application.CasosDeUso;

/// <summary>
/// Casos de uso de autenticación: login y registro de usuarios.
/// 
/// Para defender: 
/// - El login busca al usuario por correo, verifica la contraseña con BCrypt + pepper,
///   y genera un JWT con claims de Id, correo y rol.
/// - El registro valida que el correo no exista, hashea la contraseña y crea el usuario.
/// </summary>
public class AuthUC
{
    private readonly IUsuarioRepositorio _usuarioRepo;
    private readonly IServicioAuth _servicioAuth;

    public AuthUC(IUsuarioRepositorio usuarioRepo, IServicioAuth servicioAuth)
    {
        _usuarioRepo = usuarioRepo;
        _servicioAuth = servicioAuth;
    }

    public async Task<LoginRespuesta> LoginAsync(LoginPeticion peticion)
    {
        var usuario = await _usuarioRepo.ObtenerPorCorreo(peticion.CorreoElectronico)
            ?? throw new ReglaNegocioExcepcion("Credenciales inválidas.");

        if (!usuario.Activo)
            throw new ReglaNegocioExcepcion("La cuenta está desactivada.");

        if (!_servicioAuth.VerificarPassword(peticion.Password, usuario.PasswordHash))
            throw new ReglaNegocioExcepcion("Credenciales inválidas.");

        var usuarioDto = MapearADto(usuario);
        var token = _servicioAuth.GenerarToken(usuarioDto);

        return new LoginRespuesta { Token = token, Usuario = usuarioDto };
    }

    public async Task<LoginRespuesta> RegistroAsync(RegistroPeticion peticion)
    {
        if (await _usuarioRepo.ExisteCorreo(peticion.CorreoElectronico))
            throw new ReglaNegocioExcepcion("El correo electrónico ya está registrado.");

        var usuario = new Usuario
        {
            Id = Guid.NewGuid(),
            Nombre = peticion.Nombre,
            CorreoElectronico = peticion.CorreoElectronico,
            PasswordHash = _servicioAuth.HashPassword(peticion.Password),
            Rol = peticion.Rol,
            Activo = true
        };

        await _usuarioRepo.Crear(usuario);
        var usuarioDto = MapearADto(usuario);
        var token = _servicioAuth.GenerarToken(usuarioDto);
        return new LoginRespuesta { Token = token, Usuario = usuarioDto };
    }

    private static UsuarioDto MapearADto(Usuario u) => new()
    {
        Id = u.Id,
        Nombre = u.Nombre,
        CorreoElectronico = u.CorreoElectronico,
        Rol = u.Rol,
        Activo = u.Activo
    };
}
