using FluentValidation;
using FluentValidation.Results;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Contracts.Responses;
using ModernToDoList.Api.Domain.Entities;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEncryptionService _encryptionService;
    
    public AuthService(IUserRepository userRepository, IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _encryptionService = encryptionService;
    }

    public async Task<SignupResponse> SignupAsync(SignupRequest request, CancellationToken ct)
    {
        var user = SignupRequestToUserMapper.ToUser(request);

        // TODO Email Address Validation
        
        var existsByUsername = await _userRepository.FindByUsernameAsync(user.Username, ct);
        var existsByEmail = await _userRepository.FindByEmailAsync(user.EmailAddress, ct);

        if (existsByUsername is not null)
        {
            const string message = "There is already a user with that username";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }
        
        if (existsByEmail is not null)
        {
            const string message = "There is already a user with that email address";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }

        user.PasswordHash = _encryptionService.HashPassword(user.PasswordHash);
        
        await _userRepository.CreateAsync(user, ct);

        return new SignupResponse()
        {
            Token = await _encryptionService.CreateTokenAsync(user.Id, ct)
        };
    }

    public async Task<SigninResponse> SigninAsync(SigninWithUsernameRequest request, CancellationToken ct)
    {
        var user = await _userRepository.FindByUsernameAsync(request.Username, ct);

        if (user is null)
        {
            const string message = "Username is incorrect";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }

        if (!_encryptionService.ValidatePassword(request.Password, user.PasswordHash))
        {
            const string message = "Password is incorrect";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }
        
        return new SigninResponse()
        {
            Token = await _encryptionService.CreateTokenAsync(user.Id, ct)
        };
    }
    
    public async Task<SigninResponse> SigninAsync(SigninWithEmailRequest request, CancellationToken ct)
    {
        var user = await _userRepository.FindByEmailAsync(request.EmailAddress, ct);

        if (user is null)
        {
            const string message = "Email is incorrect";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }

        if (!_encryptionService.ValidatePassword(request.Password, user.PasswordHash))
        {
            const string message = "Password is incorrect";
            throw new ValidationException(message, new[]
            {
                new ValidationFailure(nameof(User), message)
            });
        }
        
        return new SigninResponse()
        {
            Token = await _encryptionService.CreateTokenAsync(user.Id, ct)
        };
    }
}