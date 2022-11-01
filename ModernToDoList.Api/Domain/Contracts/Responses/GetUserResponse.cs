namespace ModernToDoList.Api.Domain.Contracts.Responses;

public class GetUserResponse
{
    public string Id { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string EmailAddress { get; init; } = default!;
    public bool EmailAddressConfirmed { get; init; }
}