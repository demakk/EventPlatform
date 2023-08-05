using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EventPlatform.Application.Interfaces;
using EventPlatform.Application.Options;
using EventPlatform.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EventPlatform.Application.Services;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();
    
    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
    }

    public string CreateSecurityToken(ClaimsIdentity claimsIdentity, User user)
    {
        var tokenDescriptor = CreateTokenDescriptor(user, claimsIdentity);

        var token = _tokenHandler.CreateToken(tokenDescriptor);

        return WriteToken(token);
    }

    public string WriteToken(SecurityToken securityToken)
    {
        return _tokenHandler.WriteToken(securityToken);
    }

    public SecurityTokenDescriptor CreateTokenDescriptor(User user, ClaimsIdentity claimsIdentity)
    {
        return new SecurityTokenDescriptor{
            Subject = claimsIdentity,
            Issuer = _jwtSettings.Issuer,
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = 
                new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
        };
    }
}