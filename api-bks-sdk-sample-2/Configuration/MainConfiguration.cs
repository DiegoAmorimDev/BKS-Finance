using Adapters.Outbound.DataAdapter;
using bks.sdk.Common.Enums;
using bks.sdk.Common.Results;
using bks.sdk.Core.Initialization;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Ports.Outbound;
using Domain.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration;

/// <summary>
/// Classe de extensão central para configurar as dependências da aplicação.
/// </summary>
public static class MainConfiguration
{
    // Método renomeado para seguir o padrão do exemplo
    public static IServiceCollection ConfigureApiInjections(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Configuração do BKS Framework (AGORA CORRETA)
        // O SDK é configurado através de propriedades no objeto 'options',
        // e não por chamadas de método.
        services.AddBKSFramework(configuration, options =>
        {
            // Definimos o modo de processamento padrão como Mediator
            options.ProcessingMode = ProcessingMode.Mediator;
        });

        // 2. REGISTO DAS NOSSAS DEPENDÊNCIAS (para o nosso teste de "Cadastrar Produto")
        services.AddScoped<IBKSRequestHandler<CadastrarProdutoCommand, CadastrarProdutoResponse>, CadastrarProdutoCommandHandler>();
        services.AddSingleton<IProdutoRepository, InMemoryProdutoRepository>();

        return services;
    }
}
