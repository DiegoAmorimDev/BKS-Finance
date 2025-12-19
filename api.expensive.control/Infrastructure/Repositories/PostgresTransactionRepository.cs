using Dapper;
using Domain.Core.Entities;
using Domain.Core.Ports.Outbound;
using Domain.Core.Commands; // Para os DTOs de resposta nos relatórios
using Adapters.Outbound.Database.SQL;


namespace Infrastructure.Repositories
{
    public class PostgresTransactionRepository : BaseSQLRepository, ITransactionRepository
    {
        public PostgresTransactionRepository(ISQLConnectionAdapter adapter) : base(adapter) { }

        public async Task AddAsync(Transaction transaction)
        {
            await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                
                // O Banco de Dados usará o valor DEFAULT (NOW()).
                const string sql = @"
                INSERT INTO transactions (id, description, value, type, category_id, person_id) 
                VALUES (@Id, @Description, @Value, @Type, @CategoryId, @PersonId)";

                await conn.ExecuteAsync(sql, new
                {
                    transaction.Id,
                    transaction.Description,
                    transaction.Value,
                    Type = (int)transaction.Type,
                    transaction.CategoryId,
                    transaction.PersonId
                });
            });
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                // O Dapper irá ignorar a coluna created_at se ela não existir na classe Transaction
                return await conn.QueryAsync<Transaction>("SELECT * FROM transactions");
            });
        }

        public async Task<IEnumerable<Transaction>> GetByPersonIdAsync(Guid personId)
        {
            return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                return await conn.QueryAsync<Transaction>(
                    "SELECT * FROM transactions WHERE person_id = @personId", new { personId });
            });
        }

        public async Task DeleteByPersonIdAsync(Guid personId)
        {
            await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                await conn.ExecuteAsync("DELETE FROM transactions WHERE person_id = @personId", new { personId });
            });
        }

        public async Task<IEnumerable<dynamic>> GetTotalsByPersonAsync()
        {
            return await _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                const string sql = @"
                SELECT 
                    p.name AS Name,
                    COALESCE(SUM(CASE WHEN t.type = 1 THEN t.value ELSE 0 END), 0) AS TotalIncome,
                    COALESCE(SUM(CASE WHEN t.type = 0 THEN t.value ELSE 0 END), 0) AS TotalExpense
                FROM persons p
                LEFT JOIN transactions t ON p.id = t.person_id
                GROUP BY p.id, p.name";

                return await conn.QueryAsync(sql);
            });
        }

        public Task<IEnumerable<dynamic>> GetTotalsByCategoryAsync()
        {
            return _dbAdapter.ExecuteWithRetryAsync(async (conn) =>
            {
                const string sql = @"
                SELECT 
                    c.description AS Category,
                    SUM(t.value) AS Total
                FROM categories c
                JOIN transactions t ON c.id = t.category_id
                GROUP BY c.id, c.description";

                return await conn.QueryAsync(sql);
            });
        }
    }
}
