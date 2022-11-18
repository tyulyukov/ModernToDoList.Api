namespace ModernToDoList.Api.Domain.Providers;

public interface IStorageProvider
{
    Task<string> PersistFileAsync(string fileName, Stream fileStream, CancellationToken ct);
}