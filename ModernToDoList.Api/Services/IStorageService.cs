using ModernToDoList.Api.Domain;

namespace ModernToDoList.Api.Services;

public interface IStorageService
{
    bool ValidateImage(IFormFile file);
    Task<ImageAttachment> UploadImageAsync(IFormFile file, string authorId);
}