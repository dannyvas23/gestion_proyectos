
using GestionProyectos.Application.CasosDeUso;
using Xunit;

namespace Tests
{

    /// <summary>
    /// TEST: Cálculo de nueva posición al reordenar tarea.
    /// </summary>
    public class CalcularPosicionTests
    {
        [Fact]
        public void CalcularNuevaPosicion_ListaVacia_Retorna1000()
        {
            // Arrange: no hay tareas en la columna
            var ordenesExistentes = new List<double>();

            // Act
            var resultado = TareaUC.CalcularNuevaPosicion(ordenesExistentes, 0);

            // Assert: debe ser el GAP_INICIAL (1000)
            Assert.Equal(1000.0, resultado);
        }

        [Fact]
        public void CalcularNuevaPosicion_InsertarAlInicio_RetornaMitadDelPrimero()
        {
            // Arrange: hay tareas con órdenes 1000, 2000, 3000
            var ordenesExistentes = new List<double> { 1000, 2000, 3000 };

            // Act: insertar en posición 0 (antes de la primera)
            var resultado = TareaUC.CalcularNuevaPosicion(ordenesExistentes, 0);

            // Assert: debe ser 1000 / 2 = 500
            Assert.Equal(500.0, resultado);
        }

        [Fact]
        public void CalcularNuevaPosicion_InsertarAlFinal_RetornaUltimaMas1000()
        {
            // Arrange
            var ordenesExistentes = new List<double> { 1000, 2000, 3000 };

            // Act: insertar después de la última (posición 3 o más)
            var resultado = TareaUC.CalcularNuevaPosicion(ordenesExistentes, 3);

            // Assert: debe ser 3000 + 1000 = 4000
            Assert.Equal(4000.0, resultado);
        }

        [Fact]
        public void CalcularNuevaPosicion_InsertarEnMedio_RetornaPromedioEntreVecinos()
        {
            // Arrange
            var ordenesExistentes = new List<double> { 1000, 2000, 3000 };

            // Act: insertar en posición 1 (entre 1000 y 2000)
            var resultado = TareaUC.CalcularNuevaPosicion(ordenesExistentes, 1);

            // Assert: debe ser (1000 + 2000) / 2 = 1500
            Assert.Equal(1500.0, resultado);
        }
    }
}