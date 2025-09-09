using Domain.Core.Ports.Outbound;
using Domain.Core.Queries;
using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases;

public class GetProdutoByIdQueryHandler : IBKSRequestHandler<GetProdutoByIdQuery, GetProdutoByIdResponse>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<GetProdutoByIdQueryHandler> _logger;

    public GetProdutoByIdQueryHandler(IProdutoRepository produtoRepository, ILogger<GetProdutoByIdQueryHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<Result<GetProdutoByIdResponse>> HandleAsync(GetProdutoByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Buscando produto com ID {ProdutoId}", request.ProdutoId);

        var produto = await _produtoRepository.GetByIdAsync(request.ProdutoId, cancellationToken);

        if (produto is null)
        {
            _logger.LogWarning("Handler: Produto com ID {ProdutoId} não encontrado.", request.ProdutoId);
            return Result<GetProdutoByIdResponse>.Failure("Produto não encontrado.");
        }

        var response = new GetProdutoByIdResponse
        {
            Id = produto.Id,
            Codigo = produto.Codigo,
            Quantidade = produto.Quantidade,
            Preco = produto.Preco
        };

        return Result<GetProdutoByIdResponse>.Success(response);
    }
}