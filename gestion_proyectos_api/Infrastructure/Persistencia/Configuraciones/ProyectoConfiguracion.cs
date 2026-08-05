using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Configuraciones;

public class ProyectoConfiguracion : IEntityTypeConfiguration<Proyecto>
{
    public void Configure(EntityTypeBuilder<Proyecto> builder)
    {
        builder.ToTable("proyectos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500);

        builder.Property(p => p.Estado)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Activo)
            .HasDefaultValue(true);

        // Relación: un proyecto tiene muchas columnas
        builder.HasMany(p => p.Columnas)
            .WithOne(c => c.Proyecto)
            .HasForeignKey(c => c.ProyectoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
