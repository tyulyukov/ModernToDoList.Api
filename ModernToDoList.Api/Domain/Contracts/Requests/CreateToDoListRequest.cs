namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class CreateToDoListRequest
{
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
}