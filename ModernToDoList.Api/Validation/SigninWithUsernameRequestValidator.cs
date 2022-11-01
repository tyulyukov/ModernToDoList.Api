using FastEndpoints;
using FluentValidation;
using ModernToDoList.Api.Domain.Contracts.Requests;

namespace ModernToDoList.Api.Validation;

public class SigninWithUsernameRequestValidator : Validator<SigninWithUsernameRequest>
{
    public SigninWithUsernameRequestValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Username).NotEmpty();
    }
}