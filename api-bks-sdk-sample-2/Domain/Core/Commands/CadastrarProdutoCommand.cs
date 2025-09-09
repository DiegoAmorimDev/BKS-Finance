using bks.sdk.Common.Results;
using bks.sdk.Processing.Mediator.Abstractions;

namespace Domain.Core.Commands;

public record CadastrarProdutoResponse
{
    public int ProdutoId { get; init; }
    public string Mensagem { get; init; } = string.Empty;
    public DateTime DataCadastro { get; init; }
}
// CORREÇÃO: A interface espera o tipo de resposta "desembrulhado", e não o Result<>.
public class CadastrarProdutoCommand : IBKSRequest<CadastrarProdutoResponse>
{
    public string Codigo { get; init; } = string.Empty;
    public int Quantidade { get; init; }
    public decimal Preco { get; init; }
    public string RequestId { get; init; } = string.Empty;
}

