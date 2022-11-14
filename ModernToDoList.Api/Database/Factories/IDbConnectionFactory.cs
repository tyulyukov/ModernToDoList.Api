using System.Data;

namespace ModernToDoList.Api.Database.Factories;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}