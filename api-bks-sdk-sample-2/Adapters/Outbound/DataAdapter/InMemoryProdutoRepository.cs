using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using System.Threading;
using System.Threading.Tasks;

namespace Adapters.Outbound.DataAdapter;

public class InMemoryProdutoRepository : IProdutoRepository
{
    private static int _nextId = 1;
    public Task<Produto> SalvarAsync(Produto produto, CancellationToken cancellationToken)
    {
        produto.Id = Interlocked.Increment(ref _nextId);
        return Task.FromResult(produto);
    }
}