namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SignupRequest
{
    public String Username { get; init; }
    public String Password { get; init; }
}