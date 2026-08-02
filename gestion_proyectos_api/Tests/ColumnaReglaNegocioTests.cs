using Application.Excepciones;
using Domain.Puertos;
using GestionProyectos.Application.CasosDeUso;
using GestionProyectos.Domain.Entidades;
using Moq;
using Xunit;

namespace Tests
{
    /// <summary>
    /// TEST: No se puede eliminar una columna que contiene tareas.
    /// </summary>
    public class ColumnaReglaNegocioTests
    {
        [Fact]
        public async Task EliminarColumna_ConTareas_LanzaExcepcion()
        {
            // Arrange: crear mock del repositorio
            var mockRepo = new Mock<IColumnaRepositorio>();
            var columnaId = Guid.NewGuid();

            // Simular que la columna existe
            mockRepo.Setup(r => r.ObtenerPorId(columnaId))
                .ReturnsAsync(new Columna
                {
                    Id = columnaId,
                    Nombre = "En Progreso",
                    Orden = 1,
                    ProyectoId = Guid.NewGuid(),
                    Activa = true
                });

            // Simular que la columna TIENE tareas
            mockRepo.Setup(r => r.TieneTareas(columnaId))
                .ReturnsAsync(true);

            var casoDeUso = new ColumnaUC(mockRepo.Object);

            // Act & Assert: debe lanzar ReglaNegocioExcepcion
            var excepcion = await Assert.ThrowsAsync<ReglaNegocioExcepcion>(
                () => casoDeUso.Eliminar(columnaId));

            Assert.Contains("No se puede eliminar una columna que contiene tareas", excepcion.Message);
        }

        [Fact]
        public async Task EliminarColumna_SinTareas_DesactivaColumna()
        {
            // Arrange
            var mockRepo = new Mock<IColumnaRepositorio>();
            var columnaId = Guid.NewGuid();
            var columna = new Columna
            {
                Id = columnaId,
                Nombre = "Hecho",
                Orden = 2,
                ProyectoId = Guid.NewGuid(),
                Activa = true
            };

            mockRepo.Setup(r => r.ObtenerPorId(columnaId)).ReturnsAsync(columna);
            mockRepo.Setup(r => r.TieneTareas(columnaId)).ReturnsAsync(false);
            mockRepo.Setup(r => r.Actualizar(It.IsAny<Columna>())).Returns(Task.CompletedTask);

            var casoDeUso = new ColumnaUC(mockRepo.Object);

            // Act: no debe lanzar excepción
            await casoDeUso.Eliminar(columnaId);

            // Assert: la columna debe haberse desactivado
            Assert.False(columna.Activa);
            mockRepo.Verify(r => r.Actualizar(columna), Times.Once);
        }
    }
}