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

        //SERVIÇOS DE HEALTH CHECK (não serão usados nesse sistema)
        services.AddHealthChecks();

        // Esta é responsável por configurar o SDK e as dependências da aplicação
        services.ConfigureApiInjections(configuration);

        return services;
    }

    public static void UseAPIExtensions(this WebApplication app)
    {
        app.UseSwaggerExtensions();
        app.MapPersonEndpoints();
        app.MapCategoryEndpoints();
        app.MapTransactionEndpoints();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        
        app.MapHealthChecks("/health");

        
        app.Run();
    }
}
