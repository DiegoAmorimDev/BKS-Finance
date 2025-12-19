using Dapper;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Commands; // Para os DTOs de resposta nos relatórios
using Adapters.Outbound.Database.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Infrastructure.Repositories;

public class PostgresCategoryRepository : BaseSQLRepository, ICategoryRepository
{
    public PostgresCategoryRepository(ISQLConnectionAdapter adapter) : base(adapter) { }

    public async Task AddAsync(Category category)
    {
        await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            const string sql = "INSERT INTO categories (id, description, purpose) VALUES (@Id, @Description, @Purpose)";
            await conn.ExecuteAsync(sql, new { category.Id, category.Description, Purpose = (int)category.Purpose });
        });
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            return await conn.QueryFirstOrDefaultAsync<Category>(
                "SELECT id, description, purpose FROM categories WHERE id = @id", new { id });
        });
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            return await conn.QueryAsync<Category>("SELECT id, description, purpose FROM categories");
        });
    }
}
