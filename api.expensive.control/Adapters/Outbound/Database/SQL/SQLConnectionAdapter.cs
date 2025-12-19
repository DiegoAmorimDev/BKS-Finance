using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;
using System.Data;
using System.Diagnostics;

namespace Adapters.Outbound.Database.SQL;

public class SQLConnectionAdapter : ISQLConnectionAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<SQLConnectionAdapter> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;
    private NpgsqlConnection? _connection;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private const int MaxRetries = 3;

    public SQLConnectionAdapter(IConfiguration configuration, ILogger<SQLConnectionAdapter> logger)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentException("ConnectionString 'DefaultConnection' não encontrada.");

        // Política de Retry para erros transitórios do Postgres
        _retryPolicy = Policy
            .Handle<NpgsqlException>(ex => IsTransient(ex))
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                MaxRetries,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                (ex, time, attempt, context) =>
                {
                    _logger.LogWarning("Tentativa {Attempt} falhou. Novo retry em {Time}s. Erro: {Msg}",
                        attempt, time.TotalSeconds, ex.Message);
                });
    }

    public async Task<IDbConnection> GetConnectionAsync(CancellationToken ct = default)
    {
        await _semaphore.WaitAsync(ct);
        try
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection?.Dispose();
                _connection = new NpgsqlConnection(_connectionString);
                await _connection.OpenAsync(ct);
                _logger.LogDebug("Nova conexão Postgres aberta. TraceId: {TraceId}", Activity.Current?.TraceId);
            }
            return _connection;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ExecuteWithRetryAsync(Func<IDbConnection, Task> operation, CancellationToken ct = default)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var conn = await GetConnectionAsync(ct);
            await operation(conn);
        });
    }

    public async Task<T> ExecuteWithRetryAsync<T>(Func<IDbConnection, Task<T>> operation, CancellationToken ct = default)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var conn = await GetConnectionAsync(ct);
            return await operation(conn);
        });
    }

    private bool IsTransient(NpgsqlException ex)
    {
        // Códigos de erro transitórios do Postgres (Connection failure, Timeout, etc)
        return ex.IsTransient || ex.SqlState == "57P01" || ex.SqlState == "57P03";
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}