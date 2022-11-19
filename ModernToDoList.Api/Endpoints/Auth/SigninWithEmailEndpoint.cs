using FastEndpoints;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.Auth;

public class SigninWithEmailEndpoint : Endpoint<SigninWithEmailRequest, SigninResponse>
{
    private readonly IAuthService _authService;

    public SigninWithEmailEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/auth/signin/email");
        AllowAnonymous();
        Version(1);
    }

    public override async Task HandleAsync(SigninWithEmailRequest req, CancellationToken ct)
    {
        var response = await _authService.SigninAsync(req, ct);
        await SendAsync(response, 200, ct);
    }
}