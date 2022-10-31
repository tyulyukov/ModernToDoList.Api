namespace ModernToDoList.Api.Domain;

public class User
{
    public Guid Id { get; set; }
    public String Username { get; set; }
    public String PasswordHash { get; set; }
    public String EmailAddress { get; set; }
    public bool EmailAddressConfirmed { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}