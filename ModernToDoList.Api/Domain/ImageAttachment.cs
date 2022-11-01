namespace ModernToDoList.Api.Domain;

public class ImageAttachment
{
    public string Id { get; set; }
    public string AuthorId { get; set; } = default!;
    public string ToDoListItemId { get; set; }
    public string FileName { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string BlurHash { get; set; } = default!;
}