using Dapper;
using ModernToDoList.Api.Domain.Command;
using ModernToDoList.Api.Domain.Connection;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IConnectionPool _connectionPool;
    private readonly ICommandDefinitionBuilder<User> _commandDefinitionBuilder;

    public UserRepository(IConnectionPool connectionPool, ICommandDefinitionBuilder<User> commandDefinitionBuilder)
    {
        _connectionPool = connectionPool;
        _commandDefinitionBuilder = commandDefinitionBuilder;
    }

    public async Task<bool> CreateAsync(User user, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.CreateQuery(user, ct).Build());
        return result > 0;
    }

    public async Task<User?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            _commandDefinitionBuilder.GetQuery(id, ct).Build());
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<User>(
            _commandDefinitionBuilder.GetAllQuery(ct).Build());
    }

    public async Task<bool> UpdateAsync(User user, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.UpdateQuery(user, ct).Build());
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.DeleteQuery(id, ct).Build());
        return result > 0;
    }

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            _commandDefinitionBuilder.CustomQuery(
                    @"SELECT * FROM Users WHERE Username = @Username LIMIT 1", 
                    new { Username = username },
                    ct
                ).Build());
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<User>(
            _commandDefinitionBuilder.CustomQuery(
                @"SELECT * FROM Users WHERE EmailAddress = @EmailAddress LIMIT 1", 
                new { EmailAddress = email },
                ct
            ).Build());
    }
}