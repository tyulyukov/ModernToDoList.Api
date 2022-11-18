using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;

namespace ModernToDoList.Api.Services;

public interface IAuthService
{
    Task<SignupResponse> SignupAsync(SignupRequest request, CancellationToken ct);
    Task<SigninResponse> SigninAsync(SigninWithUsernameRequest request, CancellationToken ct);
    Task<SigninResponse> SigninAsync(SigninWithEmailRequest request, CancellationToken ct);
}