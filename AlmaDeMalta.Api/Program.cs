using AlmaDeMalta.Common.Services;
using FastEndpoints;
using AlmaDeMalta.Common.DatabaseConnection;
using AlmaDeMalta.api.Middlewares;
using Serilog;
using AlmaDeMalta.Api;
using AlmaDeMalta.api;

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
    .RegisterUtilities()
    .AddFastEndpoints()
    .AuthenticationConfig(builder);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Aplicar la política de CORS configurada
if (!app.Environment.IsDevelopment())
{
    app.UseAuthentication()
       .UseAuthorization();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

// Configurar FastEndpoints en el pipeline

app.FastEndpointSetup();

app.Run();
