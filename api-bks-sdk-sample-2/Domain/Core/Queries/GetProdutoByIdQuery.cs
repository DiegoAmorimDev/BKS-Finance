using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;

namespace Domain.Core.Queries;

// A resposta que o nosso Handler retornará
public record GetProdutoByIdResponse
{
    public int Id { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public int Quantidade { get; init; }
    public decimal Preco { get; init; }
}

// CORREÇÃO: A classe agora implementa a interface IBKSRequest
public class GetProdutoByIdQuery : IBKSRequest<GetProdutoByIdResponse>
{
    public int ProdutoId { get; init; }
}