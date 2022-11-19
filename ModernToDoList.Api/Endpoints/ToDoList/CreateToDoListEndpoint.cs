using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.ToDoList;

public class CreateToDoListEndpoint : Endpoint<CreateToDoListRequest, CreateToDoListResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IToDoListService _toDoListService;

    public CreateToDoListEndpoint(IUserRepository userRepository, IToDoListService toDoListService)
    {
        _userRepository = userRepository;
        _toDoListService = toDoListService;
    }

    public override void Configure()
    {
        Post("/api/v1/todolist/create");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        Version(1);
    }

    public override async Task HandleAsync(CreateToDoListRequest request, CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var user = await _userRepository.GetAsync(id, ct);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var list = new Domain.Entities.ToDoList
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.Title,
            Description = request.Description,
            AuthorId = user.Id
        };

        await _toDoListService.CreateListAsync(list, ct);
        await SendCreatedAtAsync<CreateToDoListEndpoint>(list, new CreateToDoListResponse { Id = list.Id }, cancellation: ct);
    }
}