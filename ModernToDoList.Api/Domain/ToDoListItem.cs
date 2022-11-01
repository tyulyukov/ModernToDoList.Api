namespace ModernToDoList.Api.Domain;

public class ToDoListItem
{
    public string Id { get; set; } = default!;
    public string ToDoListId { get; set; } = default!;
    public string ItemHeader { get; set; } = default!;
    public string Body { get; set; } = default!;
    public bool IsDone { get; set; } = default!;
    public string[] ImageAttachmentsId = default!;
}