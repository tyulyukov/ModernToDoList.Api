using FastEndpoints;
using FluentValidation;
using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Validation;

public class SigninWithEmailRequestValidator : Validator<SigninWithEmailRequest>
{
    public SigninWithEmailRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.EmailAddress).NotEmpty();
    }
}