namespace ModernToDoList.Api.Services;

public interface IEncryptionService
{
    String HashPassword(String password);
    bool ValidatePassword(String password, String passwordHash);
    Task<string> CreateTokenAsync(Guid id);
}