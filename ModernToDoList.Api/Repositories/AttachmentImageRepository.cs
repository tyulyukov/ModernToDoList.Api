using Dapper;
using ModernToDoList.Api.Database.Factories;
using ModernToDoList.Api.Domain;

namespace ModernToDoList.Api.Repositories;

public class AttachmentImageRepository : IAttachmentImageRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public AttachmentImageRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<bool> CreateAsync(ImageAttachment imageAttachment)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"INSERT INTO ImageAttachments (Id, AuthorId, FileName, Url, BlurHash)
                VALUES (@Id, @AuthorId, @FileName, @Url, @BlurHash)",
            imageAttachment);
        return result > 0;
    }

    public async Task<ImageAttachment?> GetAsync(string id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QuerySingleOrDefaultAsync<ImageAttachment>(
            @"SELECT * FROM ImageAttachments WHERE Id = @Id LIMIT 1", 
            new { Id = id.ToString() });
    }

    public async Task<IEnumerable<ImageAttachment>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        return await connection.QueryAsync<ImageAttachment>(@"SELECT * FROM ImageAttachments");
    }

    public async Task<bool> UpdateAsync(ImageAttachment imageAttachment)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(
            @"UPDATE ImageAttachments SET Id = @Id, AuthorId = @AuthorId, FileName = @FileName,
                Url = @Url, BlurHash = @BlurHash
                WHERE Id = @Id",
            imageAttachment);
        return result > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        var result = await connection.ExecuteAsync(@"DELETE FROM ImageAttachments WHERE Id = @Id",
            new {Id = id.ToString()});
        return result > 0;
    }
}