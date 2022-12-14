using Dapper;
using ModernToDoList.Api.Domain.Command;
using ModernToDoList.Api.Domain.Connection;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public class AttachmentImageRepository : IAttachmentImageRepository
{
    private readonly IConnectionPool _connectionPool;
    private readonly ICommandDefinitionBuilder<ImageAttachment> _commandDefinitionBuilder;

    public AttachmentImageRepository(IConnectionPool connectionPool, ICommandDefinitionBuilder<ImageAttachment> commandDefinitionBuilder)
    {
        _connectionPool = connectionPool;
        _commandDefinitionBuilder = commandDefinitionBuilder;
    }

    public async Task<bool> CreateAsync(ImageAttachment imageAttachment, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.CreateQuery(imageAttachment, ct).Build());
        return result > 0;
    }

    public async Task<ImageAttachment?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ImageAttachment>(
            _commandDefinitionBuilder.GetQuery(id, ct).Build());
    }

    public async Task<IEnumerable<ImageAttachment>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ImageAttachment>(
            _commandDefinitionBuilder.GetAllQuery(ct).Build());
    }

    public async Task<bool> UpdateAsync(ImageAttachment imageAttachment, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            _commandDefinitionBuilder.UpdateQuery(imageAttachment, ct).Build());
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