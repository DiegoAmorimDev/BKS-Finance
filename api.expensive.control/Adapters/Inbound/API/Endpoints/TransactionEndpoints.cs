using Adapters.Inbound.API.DTOs.Request;
using Domain.Core.Queries;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Commands.Transaction;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;

namespace Adapters.Inbound.API.Endpoints;

public static class TransactionEndpoints
{
    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1").WithTags("Transactions & Reports");

        group.MapGet("/transactions", async (IBKSMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.SendAsync(new GetAllTransactionsQuery(), ct);

            if (result == null || !result.IsSuccess)
                return Results.BadRequest(new { error = result?.Error ?? "Falha ao carregar transações" });

            return Results.Ok(result.Value);
        });

        group.MapPost("/transactions", async (
            CreateTransactionRequest request,
            FluentValidation.IValidator<CreateTransactionRequest> validator,
            IBKSMediator mediator,
            CancellationToken ct) =>
        {
            var valResult = await validator.ValidateAsync(request, ct);

            if (!valResult.IsValid)
            {
                Activity.Current?.SetStatus(ActivityStatusCode.Error, "Validation Failed");
                var errorDict = new Dictionary<string, string[]>();
                foreach (var err in valResult.Errors)
                {
                    if (errorDict.ContainsKey(err.PropertyName))
                    {
                        var list = errorDict[err.PropertyName].ToList();
                        list.Add(err.ErrorMessage);
                        errorDict[err.PropertyName] = list.ToArray();
                    }
                    else
                    {
                        errorDict[err.PropertyName] = new[] { err.ErrorMessage };
                    }
                }
                return Results.ValidationProblem(errorDict);
            }

            var command = new CreateTransactionCommand
            {
                Description = request.Description,
                Value = request.Value,
                Type = request.Type,
                CategoryId = request.CategoryId,
                PersonId = request.PersonId,
                RequestId = Guid.NewGuid().ToString("N")
            };

            var result = await mediator.SendAsync(command, ct);
            return result.IsSuccess ? Results.Created($"/api/v1/transactions/{result.Value.Id}", result.Value)
                                   : Results.BadRequest(new { error = result.Error });
        });

        group.MapGet("/reports/totals", async (IBKSMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.SendAsync(new GetTotalsByPersonQuery(), ct);

            if (result == null || !result.IsSuccess)
                return Results.Problem(result?.Error ?? "Erro interno ao gerar relatório");

            return Results.Ok(result.Value);
        });
    }
}