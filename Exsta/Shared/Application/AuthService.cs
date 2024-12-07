using Exsta_Shared.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend_Shared.Application;

public class AuthService(IConfiguration configuration) {

    private readonly IConfiguration _configuration = configuration;

    public string GenerateToken(User user) {
        var handler = new JwtSecurityTokenHandler();

        // Fetch valid API keys from your configuration or database
        var keyString = _configuration["auth-service-private-key"]
            ?? throw new InvalidOperationException("No authentication key was configured or no authentication key could be found in configuration");
        var key = Encoding.ASCII.GetBytes(keyString);

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = GenerateClaims(user),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = credentials,
        };

        var token = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(token);
    }

    private static ClaimsIdentity GenerateClaims(User user) {
        var claims = new ClaimsIdentity();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Email));

        foreach (var role in user.Roles)
            claims.AddClaim(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}
