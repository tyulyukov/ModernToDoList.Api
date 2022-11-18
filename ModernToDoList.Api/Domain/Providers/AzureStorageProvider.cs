using Azure.Storage.Blobs;

namespace ModernToDoList.Api.Domain.Providers;

public class AzureStorageProvider : IStorageProvider
{
    private readonly IConfiguration _configuration;

    public AzureStorageProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> PersistFileAsync(string fileName, Stream fileStream, CancellationToken ct)
    {
        var connectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString");
        var containerName = _configuration.GetValue<string>("AzureStorage:ContainerName");
        var container = new BlobContainerClient(connectionString, containerName);

        try
        {
            var blob = container.GetBlobClient(fileName);

            fileStream.Position = 0;
            await blob.UploadAsync(fileStream, ct);

            return blob.Uri.AbsoluteUri;
        }
        catch
        {
            throw new InvalidOperationException("Failed to upload image to storage");
        }
    }
}