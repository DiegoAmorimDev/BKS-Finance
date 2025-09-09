using Adapters.Inbound.API.Extensions;
using Configuration; // Para encontrar o ConfigureApiInjections
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Ler a Configuração ---
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// --- 2. Delegar a Configuração dos Serviços ---
// Esta única linha agora chama o MainConfiguration e o WebApiExtensions.
builder.Services.ConfigureAPI(configuration);

var app = builder.Build();

// --- 3. Delegar a Configuração do Pipeline ---
// Esta única linha agora configura o Swagger, os middlewares do SDK e os endpoints.
app.UseAPIExtensions();