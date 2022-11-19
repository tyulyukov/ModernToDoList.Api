using Dapper;
using ModernToDoList.Api.Domain.Command;
using ModernToDoList.Api.Domain.Connection;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public class ToDoListItemRepository : IToDoListItemRepository
{
    private readonly IConnectionPool _connectionPool;
    private readonly ICommandDefinitionBuilder<ToDoListItem> _commandDefinitionBuilder;

    public ToDoListItemRepository(IConnectionPool connectionPool, ICommandDefinitionBuilder<ToDoListItem> commandDefinitionBuilder)
    {
        _connectionPool = connectionPool;
        _commandDefinitionBuilder = commandDefinitionBuilder;
    }

    public async Task<bool> CreateAsync(ToDoListItem toDoListItem, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.CreateQuery(toDoListItem, ct).Build());
        return result > 0;
    }

    public async Task<ToDoListItem?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ToDoListItem>(
            _commandDefinitionBuilder.GetQuery(id, ct).Build());
    }

    public async Task<IEnumerable<ToDoListItem>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ToDoListItem>(
            _commandDefinitionBuilder.GetAllQuery(ct).Build());
    }

    public async Task<bool> UpdateAsync(ToDoListItem toDoListItem, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.UpdateQuery(toDoListItem, ct).Build());
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.DeleteQuery(id, ct).Build());
        return result > 0;
    }
}