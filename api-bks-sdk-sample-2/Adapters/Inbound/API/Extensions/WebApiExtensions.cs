using Adapters.Inbound.API.Endpoints;
using bks.sdk.Core.Initialization;
using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Adapters.Inbound.API.Extensions;

public static class WebApiExtensions
{
    public static IServiceCollection ConfigureAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.ConfigureSwagger();

        // ADICIONE ESTA LINHA PARA REGISTAR OS SERVIÇOS DE HEALTH CHECK
        services.AddHealthChecks();

        // Esta chamada continua a ser responsável por configurar o SDK e as dependências da sua aplicação
        services.ConfigureApiInjections(configuration);

        return services;
    }

    public static void UseAPIExtensions(this WebApplication app)
    {
        app.UseSwaggerExtensions();
        app.AddProdutoEndpoints(); // Renomeei para o nome correto
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        // Esta linha agora funcionará, pois os serviços foram registados
        app.MapHealthChecks("/health");

        // ADICIONE ESTA LINHA PARA INICIAR A APLICAÇÃO
        app.Run();
    }
}
