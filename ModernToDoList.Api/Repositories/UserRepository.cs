using Dapper;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Connection;

namespace ModernToDoList.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConnectionPool _connectionPool;

    public UserRepository(IConnectionPool connectionPool)
    {
        _connectionPool = connectionPool;
    }

    public async Task<bool> CreateAsync(User user, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO Users (Id, Username, PasswordHash, EmailAddress, EmailAddressConfirmed,
                ImageAttachmentId, CreatedAt, UpdatedAt)
                VALUES (@Id, @Username, @PasswordHash, @EmailAddress, @EmailAddressConfirmed,
                        @ImageAttachmentId, @CreatedAt, @UpdatedAt)",
                user, cancellationToken: ct));
        return result > 0;
    }

    public async Task<User?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(
                @"SELECT * FROM Users WHERE Id = @Id LIMIT 1",
                new { Id = id }, cancellationToken: ct));
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<User>(
            new CommandDefinition(
                @"SELECT * FROM Users", 
                cancellationToken: ct));
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            new CommandDefinition(@"UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, ImageAttachmentId = @ImageAttachmentId,
                EmailAddress = @EmailAddress, EmailAddressConfirmed = @EmailAddressConfirmed,
                CreatedAt = @CreatedAt, UpdatedAt = @UpdatedAt
                WHERE Id = @Id",
                user, cancellationToken: ct));
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(new CommandDefinition(
            @"DELETE FROM Users WHERE Id = @Id",
            new { Id = id }, cancellationToken: ct));
        return result > 0;
    }

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(
                @"SELECT * FROM Users WHERE Username = @Username LIMIT 1", 
                new { Username = username }, cancellationToken: ct));
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            new CommandDefinition(
            @"SELECT * FROM Users WHERE EmailAddress = @EmailAddress LIMIT 1", 
            new { EmailAddress = email }, cancellationToken: ct));
    }
}