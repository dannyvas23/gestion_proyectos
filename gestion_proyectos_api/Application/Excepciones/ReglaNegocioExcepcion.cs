/// <summary>
/// Excepción lanzada cuando se viola una regla de negocio.
/// </summary>
namespace Application.Excepciones
{
    public class ReglaNegocioExcepcion : Exception
    {
        public ReglaNegocioExcepcion(string mensaje) : base(mensaje) { }
    }

}
