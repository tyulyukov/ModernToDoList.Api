using System.Data;
using ModernToDoList.Api.Domain.Connection;

namespace ModernToDoList.Api.Domain.Providers;

public class ConnectionProvider : IConnectionProvider
{
    private readonly ConnectionPool _service;
    public IDbConnection Connection { get; }

    public ConnectionProvider(IDbConnection connection, ConnectionPool service)
    {
        _service = service;
        Connection = connection;
    }
    
    public void Dispose()
    {
        _service.FinalizeConnection(Connection);
    }
}