namespace ModernToDoList.Api.Domain.Contracts.Responses;

public class GetUserResponse
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string EmailAddress { get; init; }
    public bool EmailAddressConfirmed { get; init; }
}