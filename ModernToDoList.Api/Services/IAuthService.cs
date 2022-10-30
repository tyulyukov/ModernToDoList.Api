using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Services;

public interface IAuthService
{
    Task SignupAsync(SignupRequest request);
    /*Task SigninAsync(SigninRequest request);
    Task CreateToken(Guid id);*/
}