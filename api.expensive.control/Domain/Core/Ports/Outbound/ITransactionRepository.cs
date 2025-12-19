using Domain.Core.Entities;

namespace Domain.Core.Ports.Outbound;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAllAsync();
    Task DeleteByPersonIdAsync(Guid personId);

    // Para a consulta de totais exigida no teste
    Task<IEnumerable<dynamic>> GetTotalsByPersonAsync();
}