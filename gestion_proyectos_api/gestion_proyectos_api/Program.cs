using gestion_proyectos_api;
using Infrastructure.Persistencia;
using Infrastructure.SignalR;
using Microsoft.EntityFrameworkCore;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("Iniciando aplicación...\n");
Console.WriteLine("Variable Frontend:Url -> {0}\n", builder.Configuration["Frontend:Url"]);

// CORS: permitir peticiones del frontend Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins(
                builder.Configuration["Frontend:Url"] ?? "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Necesario para SignalR
    });
});

// Add services to the container.
builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- Migraciones automáticas---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

// Manejo global de excepciones
app.UseMiddleware<ManejadorExcepcionesMiddleware>();

// CORS
app.UseCors("PermitirFrontend");

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Punto de conexión para SignalR
app.MapHub<TableroHub>("/hub/tablero");

app.Run();
