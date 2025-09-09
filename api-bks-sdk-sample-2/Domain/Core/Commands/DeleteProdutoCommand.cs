using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;

namespace Domain.Core.Commands;

public class DeleteProdutoCommand : IBKSRequest<bool>
{
    public int ProdutoId { get; init; }
}