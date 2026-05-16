using Fanpa.Application.Contracts.Auth;
using FluentValidation;

namespace Fanpa.Application.Validators.Auth;

public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .Length(1, 128);
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(8, 128);
    }
}