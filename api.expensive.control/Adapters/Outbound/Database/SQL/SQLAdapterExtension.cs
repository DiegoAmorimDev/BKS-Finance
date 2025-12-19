using Domain.Core.Ports.Outbound;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adapters.Outbound.Database.SQL;

public static class SQLAdapterExtensions
{
    public static IServiceCollection AddSQLAdapter(this IServiceCollection services, IConfiguration configuration)
    {
        // Registro do Adapter com ciclo de vida Scoped (um por requisição)
        services.AddScoped<ISQLConnectionAdapter, SQLConnectionAdapter>();

        // Registro dos Repositórios usando o Adapter
        services.AddScoped<IPersonRepository, PostgresPersonRepository>();
        services.AddScoped<ITransactionRepository, PostgresTransactionRepository>();
        services.AddScoped<ICategoryRepository, PostgresCategoryRepository>();
        // Adicione os outros aqui...

        return services;
    }
}