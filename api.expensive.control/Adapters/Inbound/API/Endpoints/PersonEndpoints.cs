using Adapters.Inbound.API.DTOs.Request;
using Adapters.Inbound.API.Validators;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Queries;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;

namespace Adapters.Inbound.API.Endpoints;

public static class PersonEndpoints
{
    public static void MapPersonEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/persons").WithTags("Persons");

        // LISTAGEM (Exigência do Teste)
        group.MapGet("/", async (IBKSMediator mediator) =>
        {
            var result = await mediator.SendAsync(new GetAllPersonsQuery());
            return Results.Ok(result.Value);
        });

        // CRIAÇÃO
        group.MapPost("/", async (CreatePersonRequest request, FluentValidation.IValidator<CreatePersonRequest> validator, IBKSMediator mediator) =>
        {
            var val = await validator.ValidateAsync(request);
            if (!val.IsValid)
            {
                var activity = Activity.Current;
                activity?.SetStatus(ActivityStatusCode.Error, "Validation Failed");

                var errorDict = new Dictionary<string, string[]>();
                foreach (var error in val.Errors)
                {
                    if (errorDict.ContainsKey(error.PropertyName))
                    {
                        var list = errorDict[error.PropertyName].ToList();
                        list.Add(error.ErrorMessage);
                        errorDict[error.PropertyName] = list.ToArray();
                    }
                    else { errorDict[error.PropertyName] = new[] { error.ErrorMessage }; }
                }
                return Results.ValidationProblem(errorDict);
            }

            var result = await mediator.SendAsync(new CreatePersonCommand { Name = request.Name, Age = request.Age, RequestId = Guid.NewGuid().ToString() });
            return result.IsSuccess ? Results.Created($"/api/v1/persons/{result.Value.Id}", result.Value) : Results.BadRequest(result.Error);
        });

        // DELEÇÃO (Com Cascade Delete de Transações no Banco)
        group.MapDelete("/{id:guid}", async (Guid id, IBKSMediator mediator) =>
        {
            var result = await mediator.SendAsync(new DeletePersonCommand { Id = id, RequestId = Guid.NewGuid().ToString() });
            return result.IsSuccess ? Results.NoContent() : Results.NotFound(result.Error);
        });
    }
}
