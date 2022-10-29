using Dapper;

namespace ModernToDoList.Api.Domain.Database;

// TODO create table if not exist in repository
public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        await connection.ExecuteAsync(@"CREATE TABLE IF NOT EXISTS Users (
        Id VARCHAR (36) PRIMARY KEY, 
        Username TEXT NOT NULL,
        PasswordHash TEXT NOT NULL,
        EmailAddress TEXT NOT NULL)");
    }
}