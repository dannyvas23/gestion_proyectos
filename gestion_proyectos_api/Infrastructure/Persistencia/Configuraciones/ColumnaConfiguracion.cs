using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Configuraciones;

public class ColumnaConfiguracion : IEntityTypeConfiguration<Columna>
{
    public void Configure(EntityTypeBuilder<Columna> builder)
    {
        builder.ToTable("columnas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nombre)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Orden)
            .IsRequired();

        builder.Property(c => c.Activa)
            .HasDefaultValue(true);

        // Relación: una columna tiene muchas tareas
        builder.HasMany(c => c.Tareas)
            .WithOne(t => t.Columna)
            .HasForeignKey(t => t.ColumnaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
