using AppGuayaquil.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppGuayaquil.Application.Security;

public static class TokenGenerator
{
    public static string GenerateJwtToken(User user, IConfiguration configuration)
    {        
        var secretKey = configuration["JwtConfig:Key"];
        var issuer = configuration["JwtConfig:Issuer"];
        var audience = configuration["JwtConfig:Audience"];
        var tokenExpirationInHours = int.Parse(configuration["JwtConfig:JwtTokenExpirationInHours"]!);

        if (string.IsNullOrWhiteSpace(secretKey) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("La configuración de JWT está incompleta o falta.");
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.UserId.ToString())
        };
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(tokenExpirationInHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
