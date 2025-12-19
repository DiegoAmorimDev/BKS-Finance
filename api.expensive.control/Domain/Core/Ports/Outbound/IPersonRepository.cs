using Domain.Core.Entities;

namespace Domain.Core.Ports.Outbound;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(Guid id);
    Task AddAsync(Person person);
    Task DeleteAsync(Guid id);
}