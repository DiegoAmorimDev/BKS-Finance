using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Adapters.Outbound.DataAdapter;

public class InMemoryProdutoRepository : IProdutoRepository
{
    private static readonly ConcurrentDictionary<int, Produto> _produtos = new();
    private static int _nextId = 0;

    public Task<Produto> SalvarAsync(Produto produto, CancellationToken cancellationToken)
    {
        var newId = Interlocked.Increment(ref _nextId);
        produto.Id = newId;
        _produtos[newId] = produto;
        return Task.FromResult(produto);
    }

    public Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        _produtos.TryGetValue(id, out var produto);
        return Task.FromResult(produto);
    }

    public Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(_produtos.Values.AsEnumerable());
    }
    public Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        return Task.FromResult(_produtos.TryRemove(id, out _));
    }
}