using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Domain.Core.Entities;
using System.Collections.Generic;

namespace Domain.Core.Queries;

public class GetAllProdutosQuery : IBKSRequest<IEnumerable<GetProdutoByIdResponse>>
{
    // Este query pode não ter parâmetros, ou poderia ter parâmetros para paginação no futuro.
}
