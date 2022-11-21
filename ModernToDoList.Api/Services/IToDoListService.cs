using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Services;

public interface IToDoListService
{
    Task CreateListAsync(ToDoList toDoList, CancellationToken ct);
    Task UpdateListAsync(ToDoList toDoList, CancellationToken ct);
    Task<ToDoList?> GetListAsync(string id, string authorId, CancellationToken ct);
    Task<IEnumerable<ToDoList>> GetAllListsAsync(string authorId, CancellationToken ct);
}