namespace ModernToDoList.Api.Domain.Contracts.Responses;

public class UploadImageResponse
{
    public string Id { get; init; } = default!;
    public string FileName { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string BlurHash { get; set; } = default!;
}