using Application.Excepciones;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ManejadorExcepcionesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorExcepcionesMiddleware> _logger;

        public ManejadorExcepcionesMiddleware(RequestDelegate next, ILogger<ManejadorExcepcionesMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ReglaNegocioExcepcion ex)
            {
                _logger.LogWarning(ex, "Regla de negocio violada");
                await EscribirRespuestaError(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (NoEncontradoExcepcion ex)
            {
                _logger.LogWarning(ex, "Recurso no encontrado");
                await EscribirRespuestaError(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado");
                await EscribirRespuestaError(context, HttpStatusCode.Unauthorized, "Acceso no autorizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error interno del servidor");
                await EscribirRespuestaError(context, HttpStatusCode.InternalServerError,
                    "Ocurrió un error interno. Intente más tarde.");
            }
        }
        private static async Task EscribirRespuestaError(HttpContext context, HttpStatusCode statusCode, string mensaje)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var respuesta = JsonSerializer.Serialize(new
            {
                error = mensaje,
                statusCode = (int)statusCode
            });

            await context.Response.WriteAsync(respuesta);
        }
    }
}
