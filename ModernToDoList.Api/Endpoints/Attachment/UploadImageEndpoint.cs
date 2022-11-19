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
    private readonly IStorageImageService _storageImageService;
    private readonly IUserRepository _userRepository;

    public UploadImageEndpoint(IStorageImageService storageImageService, IUserRepository userRepository)
    {
        _storageImageService = storageImageService;
        _userRepository = userRepository;
    }

    public override void Configure()
    {
        Post("/api/v1/attachments/upload");
        AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
        AllowFileUploads();
        Version(1);
    }

    public override async Task HandleAsync(UploadImageRequest request, CancellationToken ct)
    {
        var id = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        
        var user = await _userRepository.GetAsync(id, ct);

        if (user is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        var attachment = await _storageImageService.UploadImageAsync(request.File, id, ct);
        await SendOkAsync(ImageAttachmentToUploadImageResponse.ToResponse(attachment), ct);
    }
}