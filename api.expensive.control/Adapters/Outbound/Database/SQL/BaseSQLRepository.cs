using System.Data;

namespace Adapters.Outbound.Database.SQL;

/// <summary>
/// Classe base para facilitar o uso do Adapter nos repositórios.
/// </summary>
public abstract class BaseSQLRepository
{
    protected readonly ISQLConnectionAdapter _dbAdapter;

    protected BaseSQLRepository(ISQLConnectionAdapter dbAdapter)
    {
        _dbAdapter = dbAdapter;
    }
}