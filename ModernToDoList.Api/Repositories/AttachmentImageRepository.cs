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

    public async Task<bool> CreateAsync(ImageAttachment imageAttachment)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            @"INSERT INTO ImageAttachments (Id, AuthorId, FileName, Url, BlurHash)
                VALUES (@Id, @AuthorId, @FileName, @Url, @BlurHash)",
            imageAttachment);
        return result > 0;
    }

    public async Task<ImageAttachment?> GetAsync(string id)
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QuerySingleOrDefaultAsync<ImageAttachment>(
            @"SELECT * FROM ImageAttachments WHERE Id = @Id LIMIT 1", 
            new { Id = id });
    }

    public async Task<IEnumerable<ImageAttachment>> GetAllAsync()
    {
        using var provider = _connectionPool.UseConnection();
        return await provider.Connection.QueryAsync<ImageAttachment>(@"SELECT * FROM ImageAttachments");
    }

    public async Task<bool> UpdateAsync(ImageAttachment imageAttachment)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(
            @"UPDATE ImageAttachments SET Id = @Id, AuthorId = @AuthorId, FileName = @FileName,
                Url = @Url, BlurHash = @BlurHash
                WHERE Id = @Id",
            imageAttachment);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var provider = _connectionPool.UseConnection();
        var result = await provider.Connection.ExecuteAsync(@"DELETE FROM ImageAttachments WHERE Id = @Id",
            new { Id = id });
        return result > 0;
    }
}