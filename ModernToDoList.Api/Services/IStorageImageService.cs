using ModernToDoList.Api.Domain;

namespace ModernToDoList.Api.Services;

public interface IStorageImageService
{
    Task<ImageAttachment> UploadImageAsync(IFormFile file, string authorId, CancellationToken ct);
}