using GestionTarjetaCredito.Api.Middleware;
using GestionTarjetaCredito.Application;
using GestionTarjetaCredito.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();

builder.Services.AddInfrastructure(
    builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gestión de Tarjetas de Crédito API",
        Version = "v1",
        Description = "API REST para la gestión de tarjetas de crédito, compras, pagos, movimientos mensuales, estados de cuenta y configuraciones financieras."
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "Gestión de Tarjetas de Crédito API v1");

        options.DocumentTitle =
            "Gestión de Tarjetas de Crédito API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();