using Dapper;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Connection;

namespace ModernToDoList.Api.Repositories;

public class AttachmentImageRepository : IAttachmentImageRepository
{
    private readonly IConnectionPool _connectionPool;

    public AttachmentImageRepository(IConnectionPool connectionPool)
    {
        _connectionPool = connectionPool;
    }

    public async Task<bool> CreateAsync(ImageAttachment imageAttachment, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            new CommandDefinition(
                @"INSERT INTO ImageAttachments (Id, AuthorId, FileName, Url, BlurHash)
                VALUES (@Id, @AuthorId, @FileName, @Url, @BlurHash)",
                imageAttachment, cancellationToken: ct));
        return result > 0;
    }

    public async Task<ImageAttachment?> GetAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ImageAttachment>(
            new CommandDefinition(
                @"SELECT * FROM ImageAttachments WHERE Id = @Id LIMIT 1", 
                new { Id = id }, cancellationToken: ct));
    }

    public async Task<IEnumerable<ImageAttachment>> GetAllAsync(CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ImageAttachment>(
            new CommandDefinition(
                @"SELECT * FROM ImageAttachments",
                cancellationToken: ct));
    }

    public async Task<bool> UpdateAsync(ImageAttachment imageAttachment, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            new CommandDefinition(
                @"UPDATE ImageAttachments SET Id = @Id, AuthorId = @AuthorId, FileName = @FileName,
                Url = @Url, BlurHash = @BlurHash
                WHERE Id = @Id",
                imageAttachment, cancellationToken: ct));
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken ct)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            new CommandDefinition(
                @"DELETE FROM ImageAttachments WHERE Id = @Id",
                new { Id = id }, cancellationToken: ct));
        return result > 0;
    }
}