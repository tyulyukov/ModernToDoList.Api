using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Entities;

namespace ModernToDoList.Api.Domain.Mappers;

public static class UserToGetUserMapper
{
     public static GetUserResponse ToResponse(User user)
     {
          return new GetUserResponse()
          {
               Id = user.Id,
               EmailAddress = user.EmailAddress,
               Username = user.Username,
               EmailAddressConfirmed = user.EmailAddressConfirmed
          };
     }
}