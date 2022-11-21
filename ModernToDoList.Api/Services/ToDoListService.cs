using ModernToDoList.Api.Domain.Entities;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Services;

public class ToDoListService : IToDoListService
{
    private readonly IToDoListRepository _toDoListRepository;

    public ToDoListService(IToDoListRepository toDoListRepository)
    {
        _toDoListRepository = toDoListRepository;
    }

    public async Task CreateListAsync(ToDoList toDoList, CancellationToken ct)
    {
        var res = await _toDoListRepository.CreateAsync(toDoList, ct);

        if (!res)
            throw new ApplicationException("To Do List did not created");
    }

    public async Task UpdateListAsync(ToDoList toDoList, CancellationToken ct)
    {
        var res = await _toDoListRepository.UpdateAsync(toDoList, ct);
        
        if (!res)
            throw new ApplicationException("To Do List did not updated");
    }

    public async Task<ToDoList?> GetListAsync(string id, string authorId, CancellationToken ct)
    {
        return await _toDoListRepository.GetByIdAndAuthorIdAsync(id, authorId, ct);
    }

    public async Task<IEnumerable<ToDoList>> GetAllListsAsync(string authorId, CancellationToken ct)
    {
        return await _toDoListRepository.GetAllByAuthorIdAsync(authorId, ct);
    }
}