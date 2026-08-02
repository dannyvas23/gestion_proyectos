using Application.DTOs;
using GestionProyectos.Application.CasosDeUso;
using Microsoft.AspNetCore.Mvc;

namespace GestionProyectos.API.Controllers;

/// <summary>
/// Controller para administración de usuarios.
/// Solo accesible por usuarios con rol Administrador.
/// </summary>
[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly UsuarioUC _usuarioUC;

    public UsuariosController(UsuarioUC usuarioUC)
    {
        _usuarioUC = usuarioUC;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsuarioDto>>> ObtenerTodos()
    {
        var usuarios = await _usuarioUC.ObtenerTodos();
        return Ok(usuarios);
    }
    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear([FromBody] CrearUsuarioPeticion peticion)
    {
        var usuario = await _usuarioUC.Crear(peticion);
        return Ok(usuario);
    }


}
