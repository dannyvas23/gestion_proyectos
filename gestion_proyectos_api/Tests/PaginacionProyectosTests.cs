using Application.CasosDeUso;
using Domain.Entidades;
using Domain.Enums;
using Domain.Puertos;
using Moq;
using Xunit;

namespace Tests
{
    /// <summary>
    /// TEST: Paginación retorna el número correcto de proyectos.
    /// - Se mockea el repositorio para simular 25 proyectos en la BD.
    /// - Se pide la página 2 con tamaño 10.
    /// - Se verifica que se retornan 10 items, total = 25, página = 2 y totalPaginas = 3.
    /// </summary>
    public class PaginacionProyectosTests
    {
        [Fact]
        public async Task ObtenerPaginado_Pagina2Tamanio10()
        {
            // Arrange: simular 25 proyectos, página 2 con tamaño 10
            var mockRepo = new Mock<IProyectoRepositorio>();
            var proyectosPagina = Enumerable.Range(11, 10)
                .Select(i => new Proyecto
                {
                    Id = Guid.NewGuid(),
                    Nombre = $"Proyecto {i}",
                    Estado = EstadoProyecto.Activo,
                    Activo = true,
                    FechaInicio = DateTime.UtcNow
                }).ToList();

            mockRepo.Setup(r => r.ListarProyectos(2, 10, null))
                .ReturnsAsync((
                    Items: proyectosPagina,
                    Total: 25
                ));

            var casoDeUso = new ProyectoUC(mockRepo.Object);

            // Act
            var resultado = await casoDeUso.ListarProyectos(2, 10, null);

            // Assert
            Assert.Equal(10, resultado.Items.Count);
            Assert.Equal(25, resultado.Total);
            Assert.Equal(2, resultado.Pagina);
            Assert.Equal(10, resultado.Tamanio);
            Assert.Equal(3, resultado.TotalPaginas); // ceil(25/10) = 3
        }

        [Fact]
        public async Task ObtenerPaginado_ConFiltroNombre_PasaFiltroAlRepositorio()
        {
            // Arrange
            var mockRepo = new Mock<IProyectoRepositorio>();
            mockRepo.Setup(r => r.ListarProyectos(1, 10, "test"))
                .ReturnsAsync((
                    Items: new List<Proyecto>(),
                    Total: 0
                )); 

            var casoDeUso = new ProyectoUC(mockRepo.Object);

            // Act
            await casoDeUso.ListarProyectos(1, 10, "test");

            // Assert: verificar que el repositorio fue llamado con el filtro correcto
            mockRepo.Verify(r => r.ListarProyectos(1, 10, "test"), Times.Once);
        }
    }
}