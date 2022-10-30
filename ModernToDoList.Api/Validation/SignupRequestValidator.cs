using FastEndpoints;
using FluentValidation;
using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Validation;

public class SignupRequestValidator : Validator<SignupRequest>
{
    public SignupRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}