using Dapper;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Domain;

namespace ModernToDoList.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public UserRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO Users (Id, Username, PasswordHash, EmailAddress, EmailAddressConfirmed,
                ImageAttachmentId, CreatedAt, UpdatedAt)
                VALUES (@Id, @Username, @PasswordHash, @EmailAddress, @EmailAddressConfirmed,
                        @ImageAttachmentId, @CreatedAt, @UpdatedAt)",
            user);
        return result > 0;
    }

    public async Task<User?> GetAsync(string id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @"SELECT * FROM Users WHERE Id = @Id LIMIT 1", 
            new { Id = id.ToString() });
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<User>(@$"SELECT * FROM Users");
    }

    public async Task<bool> UpdateAsync(User user)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, ImageAttachmentId = @ImageAttachmentId,
                EmailAddress = @EmailAddress, EmailAddressConfirmed = @EmailAddressConfirmed,
                CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt
                WHERE Id = @Id",
            user);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@"DELETE FROM Users WHERE Id = @Id",
            new {Id = id.ToString()});
        return result > 0;
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @$"SELECT * FROM Users WHERE Username = @Username LIMIT 1", new { Username = username });
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<User>(
            @$"SELECT * FROM Users WHERE EmailAddress = @EmailAddress LIMIT 1", new { EmailAddress = email });
    }
}