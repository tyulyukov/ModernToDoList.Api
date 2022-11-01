using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Repositories;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace ModernToDoList.Api.Services;

public class EncryptionService : IEncryptionService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public EncryptionService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public string HashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt(12);
        return BCrypt.Net.BCrypt.HashPassword(password, salt);
    }

    public bool ValidatePassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
    
    public async Task<string> CreateTokenAsync(string id)
    {
        var user = await _userRepository.GetAsync(id);

        if (user is null)
        {
            const string message = "This user does not exist";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }
        
        var claims = new []
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("Username", user.Username),
            new Claim(ClaimTypes.Email, user.EmailAddress)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _configuration.GetValue<string>("JWT:Key");
        var ttl = _configuration.GetValue<long>("JWT:TTL");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMilliseconds(ttl),
            SigningCredentials = GetSigningCredentialsByKey(key),
            Issuer = _configuration.GetValue<string>("JWT:Issuer"),
            Audience = _configuration.GetValue<string>("JWT:Audience")
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private SigningCredentials GetSigningCredentialsByKey(string key)
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
    }
}