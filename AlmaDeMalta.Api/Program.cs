using AlmaDeMalta.Common.Services;
using FastEndpoints;
using AlmaDeMalta.Common.DatabaseConnection;
using AlmaDeMalta.api.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add Serilog configuration
builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console();
});

// Agregar servicios de FastEndpoints
builder.Services.AddFastEndpoints();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permitir cualquier origen
              .AllowAnyHeader() // Permitir cualquier encabezado
              .AllowAnyMethod(); // Permitir cualquier método (GET, POST, etc.)
    });
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var services = builder.Services;

services.AddOpenApi()
    .UseMongoConfig(builder.Configuration)
    .RegisterServices()
    .RegisterUtilities();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Aplicar la política de CORS configurada
app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configurar FastEndpoints en el pipeline

app.MapGroup("/api").MapFastEndpoints();

app.Run();
