namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class UploadImageRequest
{
    public IFormFile File { get; init; } = default!;
}