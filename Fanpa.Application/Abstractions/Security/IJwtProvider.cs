using Fanpa.Domain;

namespace Fanpa.Application.Abstractions.Security;

public interface IJwtProvider
{
    string GenerateJwt(User user);
}