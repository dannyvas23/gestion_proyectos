using Application.Comun;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Infrastructure.Servicios.Auth
{
    /// <summary>
    /// Implementación del servicio de autenticación.
    /// Claims (id, correo, rol) firmados con una clave secreta HMAC-SHA256.
    /// </summary>
    public class ServicioAuth : IServicioAuth
    {
        private readonly ApiSettings _settings;

        public ServicioAuth(IConfiguration config, IOptionsMonitor<ApiSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        public string GenerarToken(UsuarioDto usuario)
        {
            var jwtKey = _settings.Key ?? throw new InvalidOperationException("JWT Key no configurada");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + _settings.SaltGeneradorHash);
        }

        public bool VerificarPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password + _settings.SaltGeneradorHash, hash);
        }
    }

}
