using Blurhash.ImageSharp;
using FluentValidation;
using FluentValidation.Results;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Providers;
using ModernToDoList.Api.Repositories;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;

namespace ModernToDoList.Api.Services;

public class StorageImageService : IStorageImageService
{
    private readonly Dictionary<string, List<byte[]>> _fileSignature = new()
    {
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
    private readonly WebpEncoder _imageWebpEncoder = new()
    {
        Quality = 80,
    };
    private readonly IAttachmentImageRepository _attachmentImageRepository;
    private readonly IStorageProvider _storageProvider;

    public StorageImageService(IAttachmentImageRepository attachmentImageRepository, IStorageProvider storageProvider)
    {
        _attachmentImageRepository = attachmentImageRepository;
        _storageProvider = storageProvider;
    }

    public async Task<ImageAttachment> UploadImageAsync(IFormFile file, string authorId, CancellationToken ct)
    {
        if (!ValidateImage(file))
        {
            const string message = "Uploaded file has unsupported type";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(IFormFile), message)
            });
        }

        var fileName = DateTime.UtcNow.ToString("yyyyMMddHHmmssffffff") + Path.GetRandomFileName() + ".webp";

        using var webpImageStream = new MemoryStream();

        await using var stream = file.OpenReadStream();
        using var image = await Image.LoadAsync(stream, ct);
        /*image.Mutate(context => 
            context.Resize(image.Width / 2, image.Height / 2));*/
        await image.SaveAsWebpAsync(webpImageStream, _imageWebpEncoder);
        
        var url = await _storageProvider.PersistFileAsync(fileName, webpImageStream, ct);
        
        var attachment = new ImageAttachment()
        {
            Id = Guid.NewGuid().ToString(),
            FileName = fileName,
            AuthorId = authorId,
            Url = url,
            BlurHash = GenerateBlurHash(image)
        };

        var created = await _attachmentImageRepository.CreateAsync(attachment, ct);

        if (!created)
            throw new InvalidOperationException("Failed to create image attachment");

        return attachment;
    }
    
    private bool ValidateImage(IFormFile file)
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

    private string GenerateBlurHash(Image image)
    {
        // TODO find some algorithm for computing x and y 
        return Blurhasher.Encode(image.CloneAs<Rgba32>(), 4, 3);
    }
}