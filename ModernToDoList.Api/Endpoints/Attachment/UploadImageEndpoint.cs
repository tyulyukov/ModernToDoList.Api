using System.Security.Claims;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;
using ModernToDoList.Api.Services;

namespace ModernToDoList.Api.Endpoints.Attachment;

public class UploadImageEndpoint : Endpoint<UploadImageRequest, UploadImageResponse>
{
    private readonly IStorageService _storageService;
    private readonly IUserRepository _userRepository;

    public UploadImageEndpoint(IStorageService storageService, IUserRepository userRepository)
    {
        _storageService = storageService;
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Post("/api/v1/attachments/upload");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        AllowFileUploads();
    }

    public override async Task HandleAsync(UploadImageRequest request, CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var attachment = await _storageService.UploadImageAsync(request.File, id);
        await SendOkAsync(ImageAttachmentToUploadImageResponse.ToResponse(attachment), ct);
    }
}