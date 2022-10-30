using Dapper;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Repositories.Dto;

namespace ModernToDoList.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly string _tableName = "Users";

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @$"INSERT INTO {_tableName} (Id, Username, PasswordHash, EmailAddress, EmailAddressConfirmed,
                             CreatedAt, UpdatedAt) VALUES (@Id, @Username, @PasswordHash, @EmailAddress,
                                                           @EmailAddressConfirmed, @CreatedAt, @UpdatedAt)",
            user);
        return result > 0;
    }

    public async Task<User?> GetAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @$"SELECT * FROM {_tableName} WHERE Id = @Id LIMIT 1", new { Id = id.ToString() });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>(@$"SELECT * FROM {_tableName}");
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @$"UPDATE {_tableName} SET Username = @Username, PasswordHash = @PasswordHash,
                EmailAddress = @EmailAddress, EmailAddressConfirmed = @EmailAddressConfirmed,
                CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt
                WHERE Id = @Id",
            user);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@$"DELETE FROM {_tableName} WHERE Id = @Id",
            new {Id = id.ToString()});
        return result > 0;
    }

    public async Task<UserDto?> FindByUsernameAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<UserDto>(
            @$"SELECT * FROM {_tableName} WHERE Username = @Username LIMIT 1", new { Username = username });
    }

    public async Task<UserDto?> FindByEmailAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<UserDto>(
            @$"SELECT * FROM {_tableName} WHERE EmailAddress = @EmailAddress LIMIT 1", new { EmailAddress = email });
    }
}