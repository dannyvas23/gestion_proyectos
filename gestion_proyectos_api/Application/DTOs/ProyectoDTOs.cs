using GestionProyectos.Domain.Enums;

namespace Application.DTOs
{
    public class ProyectoDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public EstadoProyecto Estado { get; set; }
        public bool Activo { get; set; }
    }

    public class CrearProyectoPeticion
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
    }

    public class ActualizarProyectoPeticion
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFinPrevista { get; set; }
        public EstadoProyecto Estado { get; set; }
    }

}
