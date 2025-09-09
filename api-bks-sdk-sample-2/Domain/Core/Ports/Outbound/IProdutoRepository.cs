using Domain.Core.Entities;

namespace Domain.Core.Ports.Outbound
{
    public interface IProdutoRepository
    {
        // Apenas um método para o nosso teste
        Task<Produto> SalvarAsync(Produto produto, CancellationToken cancellationToken);
    }
}
