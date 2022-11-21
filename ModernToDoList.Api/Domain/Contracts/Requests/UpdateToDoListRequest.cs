using Microsoft.AspNetCore.Mvc;

namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class UpdateToDoListRequest
{
    [FromRoute] public string Id { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
}