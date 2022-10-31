using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
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
    
    public async Task<string> CreateTokenAsync(Guid id)
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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Username", user.Username),
            new Claim(ClaimTypes.Email, user.EmailAddress)
        };

        var token = new JwtSecurityToken(
            signingCredentials: GetSigningCredentialsByKey(_configuration.GetValue<string>("JWT:Key")),
            expires: DateTime.UtcNow.AddMilliseconds(_configuration.GetValue<int>("JWT:TTL")),
            claims: claims,
            issuer: _configuration.GetValue<string>("JWT:Issuer"),
            audience: _configuration.GetValue<string>("JWT:Audience")
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private SigningCredentials GetSigningCredentialsByKey(string key)
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);
    }
}