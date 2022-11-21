using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public interface IToDoListRepository : ICrudRepository<ToDoList>
{
    Task<ToDoList?> GetByIdAndAuthorIdAsync(string id, string authorId, CancellationToken ct);
    Task<IEnumerable<ToDoList>> GetAllByAuthorIdAsync(string authorId, CancellationToken ct);
}