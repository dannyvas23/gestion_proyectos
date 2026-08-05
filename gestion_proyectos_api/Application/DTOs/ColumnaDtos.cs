namespace Application.DTOs;

public class ColumnaDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Orden { get; set; }
    public Guid ProyectoId { get; set; }
    public bool Activa { get; set; }
    public List<TareaDto> Tareas { get; set; } = new();
}

public class CrearColumnaPeticion
{
    public string Nombre { get; set; } = string.Empty;
    public Guid ProyectoId { get; set; }
}

public class ActualizarColumnaPeticion
{
    public string Nombre { get; set; } = string.Empty;
}


/// <summary>
/// Se envía la lista completa de IDs en el nuevo orden deseado.
/// </summary>
public class ReordenarColumnasPeticion
{
    public Guid ProyectoId { get; set; }
    public List<Guid> ColumnasOrdenadas { get; set; } = new();
}