using Adapters.Inbound.API.DTOs.Request;
using Domain.Core.Commands;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using bks.sdk.Common.Results;


namespace Adapters.Inbound.API.Endpoints;

public static class ProdutoEndpoints
{
    public static void AddProdutoEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/v1/produtos").WithTags("Produtos");

        group.MapPost("/", async (
            CadastrarProdutoRequest request,
            IBKSMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var command = new CadastrarProdutoCommand
            {
                Codigo = request.Codigo,
                Quantidade = request.Quantidade,
                Preco = request.Preco,
                RequestId = Guid.NewGuid().ToString("N")
            };

            var resultado = await mediator.SendAsync(command, cancellationToken);

            if (resultado.IsSuccess)
            {
                return Results.Ok(resultado.Value);
            }

            return Results.BadRequest(new { Erro = resultado.Error });
        })
        .WithName("CadastrarProduto");
    }
}