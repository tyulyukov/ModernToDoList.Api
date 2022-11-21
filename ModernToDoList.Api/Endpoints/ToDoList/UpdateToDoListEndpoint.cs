using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.ToDoList;

public class UpdateToDoListEndpoint : Endpoint<UpdateToDoListRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IToDoListService _toDoListService;

    public UpdateToDoListEndpoint(IUserRepository userRepository, IToDoListService toDoListService)
    {
        _userRepository = userRepository;
        _toDoListService = toDoListService;
    }

    public override void Configure()
    {
        Put("/todolist/update/{Id}");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Version(1);
    }

    public override async Task HandleAsync(UpdateToDoListRequest request, CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        var user = await _userRepository.GetAsync(id, ct);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var list = await _toDoListService.GetListAsync(request.Id, user.Id, ct);

        if (list is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        var updatedList = new Domain.Entities.ToDoList
        {
            Id = request.Id,
            Title = request.Title,
            Description = request.Description,
            AuthorId = user.Id
        };

        await _toDoListService.UpdateListAsync(updatedList, ct);
        await SendOkAsync(updatedList, ct);
    }
}