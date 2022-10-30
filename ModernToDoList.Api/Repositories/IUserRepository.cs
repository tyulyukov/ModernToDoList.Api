using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Repositories.Dto;

namespace ModernToDoList.Api.Repositories;

public interface IUserRepository : ICrudRepository<User>
{
    Task<UserDto?> FindByUsernameAsync(String username);
    Task<UserDto?> FindByEmailAsync(String email);
}