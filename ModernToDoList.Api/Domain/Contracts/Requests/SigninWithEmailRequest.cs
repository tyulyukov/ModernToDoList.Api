namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SigninWithEmailRequest
{
    public string EmailAddress { get; init; } = default!;
    public string Password { get; init; } = default!;
}