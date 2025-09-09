using Domain.Core.Commands;
using Domain.Core.Ports.Outbound;
using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.UseCases;

public class DeleteProdutoCommandHandler : IBKSRequestHandler<DeleteProdutoCommand, bool>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ILogger<DeleteProdutoCommandHandler> _logger;

    public DeleteProdutoCommandHandler(IProdutoRepository produtoRepository, ILogger<DeleteProdutoCommandHandler> logger)
    {
        _produtoRepository = produtoRepository;
        _logger = logger;
    }

    public async Task<Result<bool>> HandleAsync(DeleteProdutoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Recebido comando para apagar produto com ID {ProdutoId}", request.ProdutoId);

        var success = await _produtoRepository.DeleteAsync(request.ProdutoId, cancellationToken);

        if (!success)
        {
            _logger.LogWarning("Handler: Produto com ID {ProdutoId} não encontrado para deleção.", request.ProdutoId);
            return Result<bool>.Failure("Produto não encontrado.");
        }

        _logger.LogInformation("Handler: Produto com ID {ProdutoId} apagado com sucesso.", request.ProdutoId);
        return Result<bool>.Success(true);
    }
}
