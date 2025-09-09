using Domain.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Core.Ports.Outbound;

public interface IProdutoRepository
{
    Task<Produto> SalvarAsync(Produto produto, CancellationToken cancellationToken);
    Task<Produto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Produto>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
}
