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
builder.Host.UseLogging();

var services = builder.Services;

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

if (!app.Environment.IsDevelopment())
{
    app.UseAuthentication()
       .UseAuthorization();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.FastEndpointSetup();

app.Run();
