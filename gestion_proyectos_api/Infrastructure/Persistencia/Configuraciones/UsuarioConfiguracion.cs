using GestionProyectos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionProyectos.Infrastructure.Persistencia.Configuraciones;

/// <summary>
/// Configuración de la entidad Usuario.
/// - HasIndex + IsUnique asegura que no existan correos duplicados a nivel de BD.
/// - HasConversion convierte el enum RolUsuario a string en la BD para legibilidad.
/// </summary>
public class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.CorreoElectronico)
            .IsRequired()
            .HasMaxLength(150);

        builder.HasIndex(u => u.CorreoElectronico)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Rol)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(u => u.Activo)
            .HasDefaultValue(true);
    }
}
