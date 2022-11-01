namespace ModernToDoList.Api.Domain;

public class User
{
    public string Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public bool EmailAddressConfirmed { get; set; }
    public string ImageAttachmentId { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}