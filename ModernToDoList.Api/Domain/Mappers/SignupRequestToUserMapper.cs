using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Domain.Mappers;

public static class SignupRequestToUserMapper
{
     public static User ToUser(SignupRequest request)
     {
          return new User()
          {
               Id = Guid.NewGuid().ToString(),
               EmailAddress = request.EmailAddress,
               Username = request.Username,
               PasswordHash = request.Password,
               EmailAddressConfirmed = false,
               CreatedAt = DateTime.Now,
               UpdatedAt = DateTime.Now
          };
     }
}