
using System.Data;


namespace Adapters.Outbound.Database.SQL;

public interface ISQLConnectionAdapter : IAsyncDisposable
{
    Task<IDbConnection> GetConnectionAsync(CancellationToken ct = default);
    Task ExecuteWithRetryAsync(Func<IDbConnection, Task> operation, CancellationToken ct = default);
    Task<T> ExecuteWithRetryAsync<T>(Func<IDbConnection, Task<T>> operation, CancellationToken ct = default);
}

