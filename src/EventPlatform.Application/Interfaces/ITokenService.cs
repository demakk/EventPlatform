using System.Security.Claims;
using EventPlatform.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace EventPlatform.Application.Interfaces;

public interface ITokenService
{
    public string CreateSecurityToken(ClaimsIdentity claimsIdentity, User user);
    public SecurityTokenDescriptor CreateTokenDescriptor(User user, ClaimsIdentity claimsIdentity);

    public string WriteToken(SecurityToken securityToken);
}