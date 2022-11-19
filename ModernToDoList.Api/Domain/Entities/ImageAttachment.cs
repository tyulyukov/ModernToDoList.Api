namespace ModernToDoList.Api.Domain.Entities;

public class ImageAttachment : EntityBase
{
    public string AuthorId { get; set; } = default!;
    public string ToDoListItemId { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string Url { get; set; } = default!;
    public string BlurHash { get; set; } = default!;
}