namespace ModernToDoList.Api.Services;

public class EncryptionService : IEncryptionService
{
    public string HashPassword(string password)
    {
        // TODO Generate Salt
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool ValidatePassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}