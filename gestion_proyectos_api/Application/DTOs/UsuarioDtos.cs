using Domain.Enums;
namespace Application.DTOs
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }
    }

    public class CrearUsuarioPeticion
    {
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; } = RolUsuario.Miembro;
    }

    public class ActualizarUsuarioPeticion
    {
        public string Nombre { get; set; } = string.Empty;
        public string CorreoElectronico { get; set; } = string.Empty;
        public RolUsuario Rol { get; set; }
        public bool Activo { get; set; }
    }

}
