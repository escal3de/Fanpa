using System.Data;
using Fanpa.Application.Contracts.Auth;
using FluentValidation;

namespace Fanpa.Application.Validators.Auth;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 32);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Length(1, 128);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 128);
    }
}