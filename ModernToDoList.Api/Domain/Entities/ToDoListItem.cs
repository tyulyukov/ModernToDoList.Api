namespace ModernToDoList.Api.Domain.Entities;

public class ToDoListItem : EntityBase
{
    public string ToDoListId { get; set; } = default!;
    public string ItemHeader { get; set; } = default!;
    public string Body { get; set; } = default!;
    public bool IsDone { get; set; } = default!;
    public string[] ImageAttachmentsId = default!;
}