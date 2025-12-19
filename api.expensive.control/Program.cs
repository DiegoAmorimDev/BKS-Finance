using Adapters.Inbound.API.Extensions;
using Configuration;
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // URL do seu React/Vite
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// --- 2. Delegar a Configuração dos Serviços ---
builder.Services.ConfigureAPI(configuration);

var app = builder.Build();

app.UseCors(); // Deve vir ANTES do app.UseAPIExtensions()

// --- 3. Delegar a Configuração do Pipeline ---
app.UseAPIExtensions();