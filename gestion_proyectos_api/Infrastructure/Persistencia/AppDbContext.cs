using Application.Comun;
using GestionProyectos.Domain.Entidades;
using GestionProyectos.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GestionProyectos.Infrastructure.Persistencia;

/// <summary>
/// Contexto de Entity Framework Core.
/// </summary>
public class AppDbContext : DbContext
{
    private readonly ApiSettings _settings;

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptionsMonitor<ApiSettings> settings) : base(options)
    {
        _settings = settings.CurrentValue;
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Proyecto> Proyectos => Set<Proyecto>();
    public DbSet<Columna> Columnas => Set<Columna>();
    public DbSet<Tarea> Tareas => Set<Tarea>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar todas las configuraciones del ensamblado actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Registros semilla de  Usuarios
        // Contraseñas: Admin123! y Miembro123! (hasheadas con BCrypt + pepper)
        var adminId = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890");
        var miembroId = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f12345678901");

        modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                Id = adminId,
                Nombre = "Administrador",
                CorreoElectronico = "admin@gestion.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!" + _settings.SaltGeneradorHash),
                Rol = RolUsuario.Administrador,
                Activo = true
            },
            new Usuario
            {
                Id = miembroId,
                Nombre = "Miembro",
                CorreoElectronico = "miembro@gestion.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Miembro123!" + _settings.SaltGeneradorHash),
                Rol = RolUsuario.Miembro,
                Activo = true
            }
        );
    }
}
