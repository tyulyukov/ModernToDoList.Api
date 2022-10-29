using FastEndpoints;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Endpoints.Auth;

public class SignupEndpoint : Endpoint<SignupRequest>
{
    private readonly IUserRepository _userRepository;

    public SignupEndpoint(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Post("/api/v1/auth/signup");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SignupRequest req, CancellationToken ct)
    {
        // TODO mapper for SignupRequest and User
        await _userRepository.CreateAsync(new User
        {
            Id = Guid.NewGuid(),
            Username = req.Username,
            PasswordHash = req.Password,
            EmailAddress = req.EmailAddress
        });

        await SendAsync("Hello", 200, ct);
    }
}