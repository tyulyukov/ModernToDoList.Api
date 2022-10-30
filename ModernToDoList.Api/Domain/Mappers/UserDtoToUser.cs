using ModernToDoList.Api.Repositories.Dto;

namespace ModernToDoList.Api.Domain.Mappers;

public static class UserDtoToUser
{
     public static User ToUser(UserDto dto)
     {
          return new User()
          {
               Id = Guid.Parse(dto.Id),
               EmailAddress = dto.EmailAddress,
               Username = dto.Username,
               PasswordHash = dto.PasswordHash,
               EmailAddressConfirmed = dto.EmailAddressConfirmed,
               CreatedAt = dto.CreatedAt,
               UpdatedAt = dto.UpdatedAt
          };
     }
}