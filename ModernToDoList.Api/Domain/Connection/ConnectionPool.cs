using System.Collections.Concurrent;
using System.Data;
using ModernToDoList.Api.Domain.Providers;

namespace ModernToDoList.Api.Domain.Connection;

public class ConnectionPool : IConnectionPool
{
    private readonly ConcurrentBag<IDbConnection> _connections;

    public ConnectionPool(IEnumerable<IDbConnection> connections)
    {
        _connections = new ConcurrentBag<IDbConnection>(connections);
    }
  
    public IConnectionProvider UseConnection()
    {
        if (!_connections.TryTake(out var result))
            throw new ApplicationException("No available connections");
        
        return new ConnectionProvider(result, this);
    }

    internal void FinalizeConnection(IDbConnection connection) => _connections.Add(connection);
  
    public void Dispose()
    {
        while (!_connections.IsEmpty)
            if (_connections.TryTake(out var c))
                c.Dispose();
    }
}