using Domain.Enums;
namespace Application.DTOs
{
    public class LoginPeticion
    {
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegistroPeticion
    {
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; } = RolUsuario.Miembro;
    }
     
    public class LoginRespuesta
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = null!;
    }
}
