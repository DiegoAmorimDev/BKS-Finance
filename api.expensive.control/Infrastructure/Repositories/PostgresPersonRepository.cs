using Dapper;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Commands;
using Adapters.Outbound.Database.SQL;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Infrastructure.Repositories;

public class PostgresPersonRepository : BaseSQLRepository, IPersonRepository
{
    public PostgresPersonRepository(ISQLConnectionAdapter adapter) : base(adapter) { }

    public async Task AddAsync(Person person)
    {
        await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            const string sql = "INSERT INTO persons (id, name, age) VALUES (@Id, @Name, @Age)";
            await conn.ExecuteAsync(sql, new { person.Id, person.Name, person.Age });
        });
    }

    public async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            return await conn.QueryFirstOrDefaultAsync<Person>(
                "SELECT id, name, age FROM persons WHERE id = @id", new { id });
        });
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            return await conn.QueryAsync<Person>("SELECT id, name, age FROM persons");
        });
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
        {
            // O Cascade Delete no banco cuidará das transações se configurado no SQL
            await conn.ExecuteAsync("DELETE FROM persons WHERE id = @id", new { id });
        });
    }
}
