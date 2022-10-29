using System.Data;

namespace ModernToDoList.Api.Database.Factories;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}