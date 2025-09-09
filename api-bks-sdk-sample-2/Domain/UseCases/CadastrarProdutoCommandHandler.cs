using Domain.Core.Commands;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;
using Microsoft.Extensions.Logging;

namespace Domain.UseCases;

// CORREÇÃO: A interface agora espera o tipo de resposta "desembrulhado", CadastrarProdutoResponse.
public class CadastrarProdutoCommandHandler : IBKSRequestHandler<CadastrarProdutoCommand, CadastrarProdutoResponse>
{
    private readonly ILogger<CadastrarProdutoCommandHandler> _logger;
    private readonly IProdutoRepository _produtoRepository;

    public CadastrarProdutoCommandHandler(ILogger<CadastrarProdutoCommandHandler> logger, IProdutoRepository produtoRepository)
    {
        _logger = logger;
        _produtoRepository = produtoRepository;
    }

    // O tipo de retorno do método, no entanto, continua a ser Task<Result<...>>
    public async Task<Result<CadastrarProdutoResponse>> HandleAsync(CadastrarProdutoCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handler: Recebido comando para cadastrar produto com código {Codigo}", request.Codigo);

        if (request.Preco <= 0)
        {
            _logger.LogError("Handler: Tentativa de cadastrar produto com preço inválido.");
            return Result<CadastrarProdutoResponse>.Failure("O preço do produto deve ser positivo.");
        }

        var novoProduto = new Produto
        {
            Codigo = request.Codigo,
            Quantidade = request.Quantidade,
            Preco = request.Preco
        };

        var produtoSalvo = await _produtoRepository.SalvarAsync(novoProduto, cancellationToken);

        var response = new CadastrarProdutoResponse
        {
            ProdutoId = produtoSalvo.Id,
            Mensagem = $"Produto {request.Codigo} cadastrado com sucesso!",
            DataCadastro = DateTime.UtcNow
        };

        _logger.LogInformation("Handler: Produto processado com sucesso. RequestId: {RequestId}", request.RequestId);
        return Result<CadastrarProdutoResponse>.Success(response);
    }
}