using Application.DTOs;
using Application.CasosDeUso;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    /// <summary>
    /// Controller de autenticación.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUC _authUC;

        public AuthController(AuthUC authUC)
        {
            _authUC = authUC;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRespuesta>> Login([FromBody] LoginPeticion peticion)
        {
            var resultado = await _authUC.LoginAsync(peticion);
            return Ok(resultado);
        }

        [HttpPost("registro")]
        public async Task<ActionResult<LoginRespuesta>> Registro([FromBody] RegistroPeticion peticion)
        {
            var resultado = await _authUC.RegistroAsync(peticion);
            return CreatedAtAction(nameof(Login), resultado);
        }
    }
}
