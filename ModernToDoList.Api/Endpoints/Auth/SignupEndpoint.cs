using FastEndpoints;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.Auth;

public class SignupEndpoint : Endpoint<SignupRequest, SignupResponse>
{
    private readonly IAuthService _authService;

    public SignupEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/auth/signup");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(SignupRequest req, CancellationToken ct)
    {
        var response = await _authService.SignupAsync(req, ct);
        await SendAsync(response, 200, ct);
    }
}