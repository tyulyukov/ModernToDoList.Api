using Microsoft.AspNetCore.Identity;

namespace ModernToDoList.Api.Domain;

public class AppUser
{
    public Guid Id { get; init; }
    public String Username { get; init; }
    public String PasswordHash { get; init; }
    public 
}