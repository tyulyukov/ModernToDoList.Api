namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SigninWithEmailRequest
{
    public String EmailAddress { get; init; }
    public String Password { get; init; }
}