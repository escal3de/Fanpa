using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Fanpa.Application.Abstractions.Security;
using Fanpa.Domain;
using Fanpa.Domain.Permissions;
using Fanpa.Infrastructure.Realisations.Security.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fanpa.Infrastructure.Realisations.Security;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public string GenerateJwt(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Email, user.Email)
        };

        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            if (MapPermissions.Map.TryGetValue(role, out var permissions))
                claims.AddRange(permissions.Select(permission => new Claim("Permissions", permission)));
        }

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.UtcNow.AddHours(_jwtOptions.ExpiresHours));

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}