using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Servicios.ComunicacionContinua;

/// <summary>
/// Hub de SignalR para comunicación en tiempo real del tablero.
/// </summary>
//[Authorize]
public class TableroHub : Hub
{
    /// <summary>
    /// Suscribir al usuario al grupo del tablero (proyectoId).
    /// Se invoca desde el frontend al abrir un tablero.
    /// </summary>
    public async Task SuscribirTablero(string proyectoId)
    {
        var nombreGrupo = $"tablero-{proyectoId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, nombreGrupo);

        var nombreUsuario = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
        var usuarioId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        // Notificar al grupo que un usuario se conectó
        await Clients.Group(nombreGrupo).SendAsync("UsuarioConectado", new
        {
            UsuarioId = usuarioId,
            Nombre = nombreUsuario,
            ConnectionId = Context.ConnectionId
        });
    }

    /// <summary>
    /// Desuscribir al usuario del grupo del tablero.
    /// Se invoca al cerrar/destruir el componente del tablero.
    /// </summary>
    public async Task DesuscribirTablero(string proyectoId)
    {
        var nombreGrupo = $"tablero-{proyectoId}";
        var nombreUsuario = Context.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Desconocido";
        var usuarioId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

        await Clients.Group(nombreGrupo).SendAsync("UsuarioDesconectado", new
        {
            UsuarioId = usuarioId,
            Nombre = nombreUsuario
        });

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, nombreGrupo);
    }

    /// <summary>
    /// Se ejecuta automáticamente cuando un cliente se desconecta.
    /// Limpieza de conexiones huérfanas.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // SignalR se encarga automáticamente de remover la conexión de todos los grupos
        await base.OnDisconnectedAsync(exception);
    }
}
