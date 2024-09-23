using MyMicroservice.Data;
using MyMicroservice.Services; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Para Swagger
using Microsoft.AspNetCore.SignalR;
using MyMicroservice.Hubs;



var builder = WebApplication.CreateBuilder(args);

// Configura Kestrel para que escuche en todas las interfaces de red en el puerto 80
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80);
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

// Agrega servicios al contenedor
builder.Services.AddControllers();

// Configurar la conexión a PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSignalR();
builder.Services.AddControllers();

var app = builder.Build();

// Habilitar CORS
app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Iniciar el consumidor de RabbitMQ
var consumer = new RabbitMQConsumer(app.Services.GetRequiredService<IServiceScopeFactory>());
consumer.StartConsuming();

app.MapHub<VisitasHub>("/hubs/visitas");

app.Run();
