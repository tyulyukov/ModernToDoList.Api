namespace ModernToDoList.Api.Domain;

public class ToDoList
{
    public string Id { get; set; } = default!;
    public string AuthorId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string[] ToDoListItems = default!;
}