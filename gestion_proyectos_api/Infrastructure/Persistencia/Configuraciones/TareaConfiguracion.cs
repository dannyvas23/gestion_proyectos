using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistencia.Configuraciones;

public class TareaConfiguracion : IEntityTypeConfiguration<Tarea>
{
    public void Configure(EntityTypeBuilder<Tarea> builder)
    {
        builder.ToTable("tareas");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Descripcion)
            .HasMaxLength(2000);

        builder.Property(t => t.Prioridad)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Orden)
            .IsRequired();

        builder.Property(t => t.FechaCreacion)
            .IsRequired();

        // Relación: una tarea puede tener un responsable (opcional)
        builder.HasOne(t => t.Responsable)
            .WithMany(u => u.TareasAsignadas)
            .HasForeignKey(t => t.ResponsableId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
