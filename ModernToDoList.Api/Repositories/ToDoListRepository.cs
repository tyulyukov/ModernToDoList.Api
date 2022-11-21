using Dapper;
using ModernToDoList.Api.Domain.Command;
using ModernToDoList.Api.Domain.Connection;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public class ToDoListRepository : IToDoListRepository
{
    private readonly IConnectionPool _connectionPool;
    private readonly ICommandDefinitionBuilder<ToDoList> _commandDefinitionBuilder;

    public ToDoListRepository(IConnectionPool connectionPool, ICommandDefinitionBuilder<ToDoList> commandDefinitionBuilder)
    {
        _connectionPool = connectionPool;
        _commandDefinitionBuilder = commandDefinitionBuilder;
    }

    public async Task<bool> CreateAsync(ToDoList toDoList, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.CreateQuery(toDoList, ct).Build());
        return result > 0;
    }

    public async Task<ToDoList?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ToDoList>(
            _commandDefinitionBuilder.GetQuery(id, ct).Build());
    }

    public async Task<IEnumerable<ToDoList>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ToDoList>(
            _commandDefinitionBuilder.GetAllQuery(ct).Build());
    }

    public async Task<bool> UpdateAsync(ToDoList toDoList, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.UpdateQuery(toDoList, ct).Build());
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.DeleteQuery(id, ct).Build());
        return result > 0;
    }

    public async Task<ToDoList?> GetByIdAndAuthorIdAsync(string id, string authorId, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ToDoList>(
            _commandDefinitionBuilder.CustomQuery(
                @"SELECT * FROM {0} WHERE (Id = @Id AND AuthorId = @AuthorId) LIMIT 1", 
                new { Id = id, AuthorId = authorId },
                ct
            ).Build());
    }

    public async Task<IEnumerable<ToDoList>> GetAllByAuthorIdAsync(string authorId, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ToDoList>(
            _commandDefinitionBuilder.CustomQuery(
                @"SELECT * FROM {0} WHERE (AuthorId = @AuthorId)", 
                new { AuthorId = authorId },
                ct
            ).Build());
    }
}