using Application.DTOs;
using Application.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Infrastructure.Servicios.Reportes
{
    public class ServicioPdf : IServicioReporte
    {
        public byte[] GenerarReporte(ProyectoDto proyecto, List<ColumnaDto> columnas)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    //ENCABEZADO
                    page.Header().Column(col =>
                    {
                        col.Item().Text($"Reporte del Proyecto: {proyecto.Nombre}")
                            .SemiBold().FontSize(18).FontColor(Colors.Blue.Darken2);

                        col.Item().PaddingTop(5).Text($"Descripción: {proyecto.Descripcion}")
                            .FontSize(10).FontColor(Colors.Grey.Darken1);

                        col.Item().PaddingTop(3).Row(row =>
                        {
                            row.RelativeItem().Text($"Estado: {proyecto.Estado}").FontSize(9);
                            row.RelativeItem().Text($"Inicio: {proyecto.FechaInicio:dd/MM/yyyy}").FontSize(9);
                            row.RelativeItem().Text($"Fin previsto: {proyecto.FechaFinPrevista?.ToString("dd/MM/yyyy") ?? "N/A"}").FontSize(9);
                        });

                        col.Item().PaddingTop(3).Text($"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(8).FontColor(Colors.Grey.Medium);

                        col.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                    });

                    //Tareas del proyecto en formato de tabla
                    page.Content().PaddingVertical(10).Table(table =>
                    {
                        //columnas de la tabla
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(2);  // Columna del tablero
                            columns.RelativeColumn(3);  // Tarea
                            columns.RelativeColumn(2);  // Responsable
                            columns.RelativeColumn(1);  // Prioridad
                        });

                        // Encabezados
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                .Text("Columna").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                .Text("Tarea").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                .Text("Responsable").FontColor(Colors.White).SemiBold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                                .Text("Prioridad").FontColor(Colors.White).SemiBold();
                        });

                        // Filas de datos
                        var filaAlterna = false;
                        foreach (var columna in columnas)
                        {
                            foreach (var tarea in columna.Tareas)
                            {
                                var fondo = filaAlterna ? Colors.Grey.Lighten4 : Colors.White;

                                table.Cell().Background(fondo).Padding(4).Text(columna.Nombre);
                                table.Cell().Background(fondo).Padding(4).Text(tarea.Titulo);
                                table.Cell().Background(fondo).Padding(4).Text(tarea.ResponsableNombre ?? "Sin asignar");
                                table.Cell().Background(fondo).Padding(4).Text(tarea.Prioridad.ToString());

                                filaAlterna = !filaAlterna;
                            }
                        }
                    });
                });
            });

            return documento.GeneratePdf();
        }

    }
}