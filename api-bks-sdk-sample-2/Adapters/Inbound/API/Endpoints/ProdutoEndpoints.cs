using Adapters.Inbound.API.DTOs.Request;
using Domain.Core.Commands;
using Domain.Core.Queries;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using bks.sdk.Common.Results;
using System;
using System.Threading;

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

        group.MapGet("/{id:int}", async (
            int id,
            IBKSMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetProdutoByIdQuery { ProdutoId = id };

            var resultado = await mediator.SendAsync(query, cancellationToken);

            if (resultado.IsSuccess)
            {
                return Results.Ok(resultado.Value);
            }

            return Results.NotFound(new { Erro = resultado.Error });
        })
        .WithName("GetProdutoById");

        group.MapGet("/", async (
            IBKSMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllProdutosQuery();

            var resultado = await mediator.SendAsync(query, cancellationToken);

            if (resultado.IsSuccess)
            {
                return Results.Ok(resultado.Value);
            }

            return Results.Problem(resultado.Error);
        })
        .WithName("GetAllProdutos");

        group.MapDelete("/{id:int}", async (
            int id,
            IBKSMediator mediator,
            CancellationToken cancellationToken) =>
                {
                    var command = new DeleteProdutoCommand { ProdutoId = id };

                    var resultado = await mediator.SendAsync(command, cancellationToken);

                    // CORREÇÃO: O valor de um Result<bool> é um booleano.
                    // A verificação correta é se a operação foi um sucesso E se o valor retornado é 'true'.
                    if (resultado.IsSuccess && resultado.Value)
                    {
                        return Results.NoContent();
                    }

                    return Results.NotFound(new { Erro = resultado.Error });
                })
        .WithName("DeleteProduto");
    }
}
