using Domain.Core.Entities;


namespace Domain.Core.Ports.Outbound;

public interface ICategoryRepository
{
    Task AddAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<IEnumerable<Category>> GetAllAsync();
}