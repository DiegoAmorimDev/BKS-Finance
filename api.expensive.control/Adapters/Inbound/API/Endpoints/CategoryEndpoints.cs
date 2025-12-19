using Adapters.Inbound.API.DTOs.Request;
using bks.sdk.Processing.Mediator.Abstractions;
using bks.sdk.Validation.Abstractions;
using Domain.Core.Commands;
using Domain.Core.Queries;
using FluentValidation;
using FluentValidation.Results; // <--- OBRIGATÓRIO PARA RECONHECER ValidationFailure
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Adapters.Inbound.API.Endpoints;
public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/categories").WithTags("Categories");

        // LISTAGEM DE CATEGORIAS
        group.MapGet("/", async (IBKSMediator mediator) =>
        {
            var result = await mediator.SendAsync(new GetAllCategoriesQuery());
            return Results.Ok(result.Value);
        });

        // CRIAÇÃO DE CATEGORIA
        group.MapPost("/", async (
            CreateCategoryRequest request,
            FluentValidation.IValidator<CreateCategoryRequest> validator,
            IBKSMediator mediator,
            CancellationToken ct) =>
        {
            // Validação manual necessária para Minimal APIs
            var validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                // Enriquecimento de Telemetria (Tracing/Métricas)
                var activity = Activity.Current;
                activity?.SetStatus(ActivityStatusCode.Error, "Validation Failed");
                activity?.AddTag("otel.status_code", "ERROR");

                // Mapeamento manual de erros para evitar conflitos de tipos (LINQ CS1929)
                var errorDict = new Dictionary<string, string[]>();
                foreach (var error in validationResult.Errors)
                {
                    if (errorDict.ContainsKey(error.PropertyName))
                    {
                        var currentErrors = errorDict[error.PropertyName].ToList();
                        currentErrors.Add(error.ErrorMessage);
                        errorDict[error.PropertyName] = currentErrors.ToArray();
                    }
                    else
                    {
                        errorDict[error.PropertyName] = new[] { error.ErrorMessage };
                    }
                }
                return Results.ValidationProblem(errorDict);
            }

            var command = new CreateCategoryCommand
            {
                Description = request.Description,
                Purpose = request.Purpose,
                RequestId = Guid.NewGuid().ToString("N")
            };

            var result = await mediator.SendAsync(command, ct);

            return result.IsSuccess
                ? Results.Created($"/api/v1/categories/{result.Value.Id}", result.Value)
                : Results.BadRequest(new { error = result.Error });
        });
    }
}