using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Repositories.Dto;

namespace ModernToDoList.Api.Domain.Mappers;

public static class UserDtoToGetUserResponseMapper
{
     public static GetUserResponse ToGetUserResponse(UserDto user)
     {
          return new GetUserResponse()
          {
               Id = Guid.Parse(user.Id),
               EmailAddress = user.EmailAddress,
               Username = user.Username,
               EmailAddressConfirmed = user.EmailAddressConfirmed,
          };
     }
}