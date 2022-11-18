namespace ModernToDoList.Api.Services;

public interface IEncryptionService
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string passwordHash);
    Task<string> CreateTokenAsync(string id, CancellationToken ct);
}