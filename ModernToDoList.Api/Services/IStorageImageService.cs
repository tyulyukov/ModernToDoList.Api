using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Services;

public interface IStorageImageService
{
    Task<ImageAttachment> UploadImageAsync(IFormFile file, string authorId, CancellationToken ct);
}