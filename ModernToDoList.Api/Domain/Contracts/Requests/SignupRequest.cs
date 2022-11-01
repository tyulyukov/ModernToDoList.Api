namespace ModernToDoList.Api.Domain.Contracts.Requests;

public class SignupRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string EmailAddress { get; init; } = default!;
}