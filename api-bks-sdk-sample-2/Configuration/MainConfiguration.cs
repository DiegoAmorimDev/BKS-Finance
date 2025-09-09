using Adapters.Outbound.DataAdapter;
using bks.sdk.Common.Enums;
using bks.sdk.Core.Initialization;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;
using Domain.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Configuration;

public static class MainConfiguration
{
    public static IServiceCollection ConfigureApiInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBKSFramework(configuration, options =>
        {
            options.ProcessingMode = ProcessingMode.Mediator;
        });

        // REGISTO DAS NOSSAS DEPENDÊNCIAS
        services.AddScoped<IBKSRequestHandler<CadastrarProdutoCommand, CadastrarProdutoResponse>, CadastrarProdutoCommandHandler>();
        services.AddScoped<IBKSRequestHandler<GetProdutoByIdQuery, GetProdutoByIdResponse>, GetProdutoByIdQueryHandler>();
        services.AddScoped<IBKSRequestHandler<GetAllProdutosQuery, IEnumerable<GetProdutoByIdResponse>>, GetAllProdutosQueryHandler>();
        services.AddScoped<IBKSRequestHandler<DeleteProdutoCommand, bool>, DeleteProdutoCommandHandler>();

        services.AddSingleton<IProdutoRepository, InMemoryProdutoRepository>();

        return services;
    }
}
