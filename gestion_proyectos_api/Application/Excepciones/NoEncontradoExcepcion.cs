namespace Application.Excepciones
{
    public class NoEncontradoExcepcion : Exception
    {
        public NoEncontradoExcepcion(string entidad, Guid id)
            : base($"{entidad} con Id '{id}' no fue encontrado.") { }
    }
}
