using AlmaDeMalta.Common.Services;
using FastEndpoints;
using AlmaDeMalta.Common.DatabaseConnection;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de FastEndpoints
builder.Services.AddFastEndpoints();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var services = builder.Services;

services.AddOpenApi()
    .UseMongoConfig(builder.Configuration)
    .RegisterServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Configurar FastEndpoints en el pipeline
app.UseFastEndpoints();

app.Run();
