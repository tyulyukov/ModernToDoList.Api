using Azure.Storage.Blobs;
using Blurhash.ImageSharp;
using FluentValidation;
using FluentValidation.Results;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ModernToDoList.Api.Services;

public class StorageService : IStorageService
{
    private readonly Dictionary<string, List<byte[]>> _fileSignature = new()
    {
        { ".gif", new List<byte[]>
            {
                new byte[] { 0x47, 0x49, 0x46, 0x38 }
            } 
        },
        { ".png", new List<byte[]>
            {
                new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
            } 
        },
        { ".jpeg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
            }
        },
        { ".jpg", new List<byte[]>
            {
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
            }
        },
    };

    private readonly IAttachmentImageRepository _attachmentImageRepository;
    private readonly IConfiguration _configuration;

    public StorageService(IAttachmentImageRepository attachmentImageRepository, IConfiguration configuration)
    {
        _attachmentImageRepository = attachmentImageRepository;
        _configuration = configuration;
    }

    public bool ValidateImage(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!_fileSignature.ContainsKey(extension))
            return false;
        
        var signatures = _fileSignature[extension];

        using var stream = file.OpenReadStream();
        using var reader = new BinaryReader(stream);
        var headerBytes = reader.ReadBytes(signatures.Max(m => m.Length));

        return signatures.Any(signature => 
            headerBytes.Take(signature.Length).SequenceEqual(signature));
    }

    public async Task<ImageAttachment> UploadImageAsync(IFormFile file, string authorId)
    {
        if (!ValidateImage(file))
        {
            const string message = "Uploaded file is not an image";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(IFormFile), message)
            });
        }

        var fileName = Path.GetRandomFileName() + ".webp";

        using var imageStream = new MemoryStream();
        
        await using var stream = file.OpenReadStream();
        var image = await Image.LoadAsync(stream);
        await image.SaveAsWebpAsync(imageStream);
        
        var url = await PersistImageAsync(stream, fileName);
        
        var attachment = new ImageAttachment()
        {
            Id = Guid.NewGuid().ToString(),
            FileName = fileName,
            AuthorId = authorId,
            Url = url,
            BlurHash = GenerateBlurHash(image)
        };

        var created = await _attachmentImageRepository.CreateAsync(attachment);

        if (!created)
            throw new InvalidOperationException("Failed to create image attachment");

        return attachment;
    }
    
    private async Task<string> PersistImageAsync(Stream imageStream, string fileName)
    {
        string connectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString");
        string containerName = _configuration.GetValue<string>("AzureStorage:ContainerName");
        var container = new BlobContainerClient(connectionString, containerName);

        try
        {
            var blob = container.GetBlobClient(fileName);

            imageStream.Position = 0;
            await blob.UploadAsync(imageStream);

            return blob.Uri.AbsoluteUri;
        }
        catch
        {
            throw new InvalidOperationException("Failed to upload image to storage");
        }
    }

    private string GenerateBlurHash(Image image)
    {
        // TODO find some algorithm for computing x and y 
        return Blurhasher.Encode(image.CloneAs<Rgba32>(), 4, 3);
    }
}