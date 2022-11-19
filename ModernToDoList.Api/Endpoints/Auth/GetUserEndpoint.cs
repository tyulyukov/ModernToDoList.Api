using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Endpoints.Auth;

public class GetUserEndpoint : EndpointWithoutRequest<GetUserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserEndpoint(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Get("/auth/me");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Version(1);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var user = await _userRepository.GetAsync(id, ct);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        await SendAsync(UserToGetUserMapper.ToResponse(user), 200, ct);
    }
}