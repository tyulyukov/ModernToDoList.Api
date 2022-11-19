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
        await _toDoListRepository.CreateAsync(toDoList, ct);
    }
}