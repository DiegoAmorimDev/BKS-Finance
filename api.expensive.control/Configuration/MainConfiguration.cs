using Adapters.Inbound.API.Validators;
using Adapters.Outbound.Database.SQL;
using bks.sdk.Common.Enums;
using bks.sdk.Core.Initialization;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Commands.Transaction;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;
using Domain.UseCases;
using Domain.UseCases.QueryHandler;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Configuration;

public static class MainConfiguration
{
    public static IServiceCollection ConfigureApiInjections(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Inicialização do Framework BKS
        services.AddBKSFramework(configuration, options =>
        {
            options.ProcessingMode = ProcessingMode.Mediator;
        });

        // 2. REGISTO DE VALIDADORES (FluentValidation)
        // Nota: Requer o pacote NuGet 'FluentValidation.DependencyInjectionExtensions'
        services.AddValidatorsFromAssemblyContaining<CreatePersonRequestValidator>();

        // 3. REGISTO DOS HANDLERS (USE CASES)


        // --- Pessoas (Persons) ---
        services.AddScoped<IBKSRequestHandler<CreatePersonCommand, CreatePersonResponse>, CreatePersonCommandHandler>();
        services.AddScoped<IBKSRequestHandler<DeletePersonCommand, bool>, DeletePersonCommandHandler>();
        services.AddScoped<IBKSRequestHandler<GetAllPersonsQuery, IEnumerable<Person>>, GetAllPersonsQueryHandler>();

        // --- Categorias (Categories) ---
        services.AddScoped<IBKSRequestHandler<CreateCategoryCommand, CreateCategoryResponse>, CreateCategoryCommandHandler>();
        services.AddScoped<IBKSRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>, GetAllCategoriesQueryHandler>();

        // --- Transações (Transactions) ---
        services.AddScoped<IBKSRequestHandler<CreateTransactionCommand, CreateTransactionResponse>, CreateTransactionCommandHandler>();

        // --- Queries de Relatórios ---


        // 3. Registro dos Handlers (Use Cases)
        services.AddScoped<IBKSRequestHandler<CreatePersonCommand, CreatePersonResponse>, CreatePersonCommandHandler>();
        services.AddScoped<IBKSRequestHandler<CreateCategoryCommand, CreateCategoryResponse>, CreateCategoryCommandHandler>();
        services.AddScoped<IBKSRequestHandler<CreateTransactionCommand, CreateTransactionResponse>, CreateTransactionCommandHandler>();
        services.AddScoped<IBKSRequestHandler<GetTotalsByPersonQuery, FullReportResponse>, GetTotalsByPersonQueryHandler>();

        // 4. PERSISTÊNCIA REAL (POSTGRES)
        
        services.AddSQLAdapter(configuration);


        return services;
    }
}