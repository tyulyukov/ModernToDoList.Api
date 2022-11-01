namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SigninWithUsernameRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}