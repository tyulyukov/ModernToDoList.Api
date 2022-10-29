using FluentValidation;
using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Validators;

public class SignupRequestValidator : AbstractValidator<SignupRequest>
{
    public SignupRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
    }
}