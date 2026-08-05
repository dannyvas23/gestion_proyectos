using Application.DTOs;

namespace Application.Interfaces
{
    public interface IServicioAuth
    {
        string GenerarToken(UsuarioDto usuario);
        string HashPassword(string password);
        bool VerificarPassword(string password, string hash);
    }

}
