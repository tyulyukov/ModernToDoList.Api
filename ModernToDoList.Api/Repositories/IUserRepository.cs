using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct);
    Task<User?> FindByEmailAsync(string email, CancellationToken ct);
}