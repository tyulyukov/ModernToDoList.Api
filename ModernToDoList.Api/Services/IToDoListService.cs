using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Services;

public interface IToDoListService
{
    Task CreateListAsync(ToDoList toDoList, CancellationToken ct);
}