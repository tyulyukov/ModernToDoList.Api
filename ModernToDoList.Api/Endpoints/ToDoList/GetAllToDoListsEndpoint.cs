using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.ToDoList;

public class GetAllToDoListsEndpoint : EndpointWithoutRequest<IEnumerable<Domain.Entities.ToDoList>>
{
    private readonly IUserRepository _userRepository;
    private readonly IToDoListService _toDoListService;

    public GetAllToDoListsEndpoint(IUserRepository userRepository, IToDoListService toDoListService)
    {
        _userRepository = userRepository;
        _toDoListService = toDoListService;
    }

    public override void Configure()
    {
        Get("/todolist/all");
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

        var lists = await _toDoListService.GetAllListsAsync(user.Id, ct);
        await SendOkAsync(lists, ct);
    }
}