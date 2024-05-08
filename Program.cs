using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

// Leer la variable de entorno
//var otlpEndpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://127.0.0.1:4317";

// Configuración inicial de Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.OpenTelemetry(options =>
    {
        options.Endpoint = Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://127.0.0.1:4317";
	options.ResourceAttributes = new Dictionary<string, object>
        {
            ["service.name"] = "MiApiConSerilog"
        };
    })  // Enviar logs a OpenTelemetry
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Configura Serilog como el proveedor de logging
builder.Host.UseSerilog();

// Agregar servicios al contenedor.
builder.Services.AddControllers();  // Esto es necesario para que tu API pueda usar los controladores

// Configuración de Kestrel (opcional, si necesitas configurar puertos específicos o soporte HTTPS)
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(5181);  // Configura Kestrel para escuchar en el puerto 5181 para HTTP
    // serverOptions.ListenLocalhost(5182, listenOptions =>  // Descomentar para HTTPS
    // {
    //     listenOptions.UseHttps("path_to_certificate.pfx", "password");  // Asegúrate de tener un certificado válido
    // });
});

var app = builder.Build();

// Middleware para enrutamiento
app.UseRouting();

// Middleware para usar controladores (necesario para las APIs REST)
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  // Esto asegura que se mapean todos los controladores
});

// Configuración adicional de la app aquí...

// Corre la aplicación
app.Run();
