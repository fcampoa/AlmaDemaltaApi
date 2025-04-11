using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios de FastEndpoints
builder.Services.AddFastEndpoints();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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
