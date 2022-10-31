namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SigninWithUsernameRequest
{
    public String Username { get; init; }
    public String Password { get; init; }
}