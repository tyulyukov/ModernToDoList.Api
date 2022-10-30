using FluentValidation;
using FluentValidation.Results;
using ModernToDoList.Api.Domain;
using ModernToDoList.Api.Domain.Contracts.Requests;
using ModernToDoList.Api.Domain.Mappers;
using ModernToDoList.Api.Repositories;

namespace ModernToDoList.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task SignupAsync(SignupRequest request)
    {
        var user = SignupRequestToUserMapper.ToUser(request);

        var existsByUsername = await _userRepository.FindByUsernameAsync(user.Username);
        var existsByEmail = await _userRepository.FindByEmailAsync(user.EmailAddress);

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
        
        // TODO password encryption like user.PasswordHash = Hash(user.PasswordHash)
        
        await _userRepository.CreateAsync(user);
        
        // TODO return jwt
    }
}