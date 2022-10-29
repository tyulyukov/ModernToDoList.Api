using FastEndpoints;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Endpoints.Auth;

public class SignupEndpoint : Endpoint<SignupRequest>
{
    private IUserRepository _userRepository;

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
        /*var response = new MyResponse()
        {
            FullName = req.FirstName + " " + req.LastName,
            IsOver18 = req.Age > 18
        };*/

        await _userRepository.CreateAsync(new User
        {
            Id = Guid.NewGuid(),
            Username = "tyulyukov",
            PasswordHash = "password",
            EmailAddress = "maks@gmail.com"
        });

        await SendAsync("Hello", cancellation: ct);
    }
}