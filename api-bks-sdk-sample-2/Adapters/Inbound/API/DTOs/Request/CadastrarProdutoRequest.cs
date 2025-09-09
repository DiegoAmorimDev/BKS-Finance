namespace Adapters.Inbound.API.DTOs.Request;

public record CadastrarProdutoRequest(string Codigo, int Quantidade, decimal Preco);