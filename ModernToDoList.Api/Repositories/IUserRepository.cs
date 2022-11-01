using ModernToDoList.Api.Domain;

namespace ModernToDoList.Api.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByEmailAsync(string email);
}