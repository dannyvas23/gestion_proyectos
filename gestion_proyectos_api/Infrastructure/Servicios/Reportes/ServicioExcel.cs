using Application.DTOs;
using Application.Interfaces;
using ClosedXML.Excel;

namespace Infrastructure.Servicios.Reportes
{

    /// <summary>
    /// Generación de reportes Excel con ClosedXML.
    /// </summary>
    public class ServicioExcel : IServicioReporte
    {
        public byte[] GenerarReporte(ProyectoDto proyecto, List<ColumnaDto> columnas)
        {
            using var workbook = new XLWorkbook();
            var hoja = workbook.Worksheets.Add("Tareas del Proyecto");

            // Encabezado del proyecto 
            hoja.Cell(1, 1).Value = "Proyecto:";
            hoja.Cell(1, 2).Value = proyecto.Nombre;
            hoja.Cell(2, 1).Value = "Descripción:";
            hoja.Cell(2, 2).Value = proyecto.Descripcion;
            hoja.Cell(3, 1).Value = "Estado:";
            hoja.Cell(3, 2).Value = proyecto.Estado.ToString();
            hoja.Cell(4, 1).Value = "Fecha Inicio:";
            hoja.Cell(4, 2).Value = proyecto.FechaInicio.ToString("dd/MM/yyyy");
            hoja.Cell(5, 1).Value = "Fin Previsto:";
            hoja.Cell(5, 2).Value = proyecto.FechaFinPrevista?.ToString("dd/MM/yyyy") ?? "N/A";
            hoja.Cell(6, 1).Value = "Fecha Generación:";
            hoja.Cell(6, 2).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            // Negrita para etiquetas
            hoja.Range("A1:A6").Style.Font.Bold = true;

            // Encabezados de la tabla 
            var filaInicio = 8;
            hoja.Cell(filaInicio, 1).Value = "Columna";
            hoja.Cell(filaInicio, 2).Value = "Tarea";
            hoja.Cell(filaInicio, 3).Value = "Responsable";
            hoja.Cell(filaInicio, 4).Value = "Prioridad";

            var rangoEncabezados = hoja.Range(filaInicio, 1, filaInicio, 4);
            rangoEncabezados.Style.Font.Bold = true;
            rangoEncabezados.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            rangoEncabezados.Style.Font.FontColor = XLColor.White;

            //Datos
            var fila = filaInicio + 1;
            foreach (var columna in columnas)
            {
                foreach (var tarea in columna.Tareas)
                {
                    hoja.Cell(fila, 1).Value = columna.Nombre;
                    hoja.Cell(fila, 2).Value = tarea.Titulo;
                    hoja.Cell(fila, 3).Value = tarea.ResponsableNombre ?? "Sin asignar";
                    hoja.Cell(fila, 4).Value = tarea.Prioridad.ToString();
                    fila++;
                }
            }

            //Anchos de columna adecuados
            hoja.Column(1).Width = 20;
            hoja.Column(2).Width = 35;
            hoja.Column(3).Width = 25;
            hoja.Column(4).Width = 15;

            // Exportar a bytes
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }

}
