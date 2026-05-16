namespace Fanpa.Application.Abstractions.Security;

public interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string hashedPassword, string providedPassword);
}