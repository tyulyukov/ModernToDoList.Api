namespace ModernToDoList.Api.Domain.Entities;

public class ToDoList : EntityBase
{
    public string AuthorId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string[] ToDoListItems = default!;
}