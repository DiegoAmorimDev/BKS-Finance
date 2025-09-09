using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;
using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.UseCases;

public class GetAllProdutosQueryHandler : IBKSRequestHandler<GetAllProdutosQuery, IEnumerable<GetProdutoByIdResponse>>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetAllProdutosQueryHandler> _logger;

    public GetAllProdutosQueryHandler(IProdutoRepository produtoRepository, ILogger<GetAllProdutosQueryHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<Result<IEnumerable<GetProdutoByIdResponse>>> HandleAsync(GetAllProdutosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Buscando todos os produtos.");

        var produtos = await _produtoRepository.GetAllAsync(cancellationToken);

        var response = produtos.Select(produto => new GetProdutoByIdResponse
        {
            Id = produto.Id,
            Codigo = produto.Codigo,
            Quantidade = produto.Quantidade,
            Preco = produto.Preco
        }).ToList();

        return Result<IEnumerable<GetProdutoByIdResponse>>.Success(response);
    }
}
