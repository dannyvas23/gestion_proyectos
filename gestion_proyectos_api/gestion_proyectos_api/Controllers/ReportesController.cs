using Application.CasosDeUso;
using Application.DTOs;
using Domain.Enums;
using Infrastructure.Servicios.Reportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{

    /// <summary>
    /// Controller de reportes PDF y Excel.
    /// </summary>
    [ApiController]
    [Route("api/reportes")]
    //[Authorize]
    public class ReportesController : ControllerBase
    {
        private readonly ProyectoUC _proyectoUC;
        private readonly ColumnaUC _columnaUC;
        private readonly ServicioPdf _servicioPdf;
        private readonly ServicioExcel _servicioExcel;

        public ReportesController(
            ProyectoUC proyectoCasoDeUso,
            ColumnaUC columnaCasoDeUso,
            TareaUC tareaCasoDeUso,
            ServicioPdf servicioPdf,
            ServicioExcel servicioExcel)
        {
            _proyectoUC = proyectoCasoDeUso;
            _columnaUC = columnaCasoDeUso;
            _servicioPdf = servicioPdf;
            _servicioExcel = servicioExcel;
        }

        [HttpGet("proyectos/{proyectoId}/pdf")]
        public async Task<IActionResult> GenerarPdf(
            Guid proyectoId,
            [FromQuery] Guid? responsableId = null,
            [FromQuery] Prioridad? prioridad = null)
        {
            var (proyecto, columnas) = await ObtenerDatosReporte(proyectoId, responsableId, prioridad);
            var pdf = _servicioPdf.GenerarReporte(proyecto, columnas);

            return File(pdf,
                "application/pdf",
                $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.pdf");
        }

        [HttpGet("proyectos/{proyectoId}/excel")]
        public async Task<IActionResult> GenerarExcel(
            Guid proyectoId,
            [FromQuery] Guid? responsableId = null,
            [FromQuery] Prioridad? prioridad = null)
        {
            var (proyecto, columnas) = await ObtenerDatosReporte(proyectoId, responsableId, prioridad);
            var excel = _servicioExcel.GenerarReporte(proyecto, columnas);

            return File(excel,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Reporte_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");
        }

        /// <summary>
        /// Obtiene los datos necesarios para el reporte, aplicando filtros opcionales.
        /// </summary>
        private async Task<(ProyectoDto proyecto, List<ColumnaDto> columnas)>
            ObtenerDatosReporte(Guid proyectoId, Guid? responsableId, Prioridad? prioridad)
        {
            var proyecto = await _proyectoUC.ObtenerPorId(proyectoId);
            var columnas = await _columnaUC.ObtenerPorProyecto(proyectoId);

            // Aplicar filtros a las tareas de cada columna
            if (responsableId.HasValue || prioridad.HasValue)
            {
                foreach (var columna in columnas)
                {
                    columna.Tareas = columna.Tareas
                        .Where(t =>
                            (!responsableId.HasValue || t.ResponsableId == responsableId.Value) &&
                            (!prioridad.HasValue || t.Prioridad == prioridad.Value))
                        .ToList();
                }
            }

            return (proyecto, columnas);
        }
    }
}
