using Fanpa.Application.Abstractions.Security;
using Fanpa.Domain;
using Microsoft.AspNetCore.Identity;

namespace Fanpa.Infrastructure.Realisations.Security;

public class PasswordHasher(IPasswordHasher<User> passwordHasher) : IPasswordHasher
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    
    public string Generate(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword);

        return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
    }
}