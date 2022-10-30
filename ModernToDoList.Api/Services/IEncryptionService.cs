namespace ModernToDoList.Api.Services;

public interface IEncryptionService
{
    String HashPassword(String password);
    bool ValidatePassword(String password, String passwordHash);
}