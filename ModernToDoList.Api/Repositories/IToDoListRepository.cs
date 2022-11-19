using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public interface IToDoListRepository : ICrudRepository<ToDoList>
{
    
}