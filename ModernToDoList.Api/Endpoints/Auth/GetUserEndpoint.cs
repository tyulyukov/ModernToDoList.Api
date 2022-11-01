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
        Get("/api/v1/auth/me");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var user = await _userRepository.GetAsync(Guid.Parse(id));

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        await SendAsync(UserDtoToGetUserResponseMapper.ToGetUserResponse(user), 200, ct);
    }
}