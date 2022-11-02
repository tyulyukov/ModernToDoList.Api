using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;

namespace ModernToDoList.Api.Domain.Mappers;

public static class ImageAttachmentToUploadImageResponse
{
     public static UploadImageResponse ToResponse(ImageAttachment attachment)
     {
          return new UploadImageResponse()
          {
               Id = attachment.Id,
               Url = attachment.Url,
               BlurHash = attachment.BlurHash,
               FileName = attachment.FileName
          };
     }
}