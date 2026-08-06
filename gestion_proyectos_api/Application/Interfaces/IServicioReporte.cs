using Application.DTOs;

namespace Application.Interfaces
{
    public interface IServicioReporte
    {
        byte[] GenerarReporte(ProyectoDto proyecto, List<ColumnaDto> columnas);
    }

}
