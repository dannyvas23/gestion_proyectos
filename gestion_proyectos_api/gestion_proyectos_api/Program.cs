using gestion_proyectos_api;
using GestionProyectos.Infrastructure.Persistencia;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAPIServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("PostgresDatabase");

// Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseNpgsql(connectionString)
);



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
