using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using EventPlatform.Application.Commands.IdentityCommands;
using EventPlatform.Application.Interfaces;
using EventPlatform.Domain.Common;  
using EventPlatform.Domain.Models;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventPlatform.Application.Commands.IdentityCommandHandlers;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, OperationResultT<string>>
{
    private readonly IApplicationDbContext _ctx;
    private readonly ITokenService _tokenService;
    private readonly OperationResultT<string> _resultT = new();

    public LoginUserHandler(IApplicationDbContext ctx, ITokenService tokenService)
    {
        _ctx = ctx;
        _tokenService = tokenService;
    }

    public async Task<OperationResultT<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var connection = _ctx.CreateConnection();
        connection.Open();

        var user = await GetUserAsync(connection, request.Email);
        if (user is null)
        {
            _resultT.AddError(new Error
            {
                Code = ErrorCode.UserDoesNotExist,
                ErrorMessage = "User with such email does not exist"
            });
            return _resultT;
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                _resultT.AddError(new Error { Code = ErrorCode.WrongPassword, ErrorMessage = "Invalid password" });
                return _resultT;
            }
        }

        _resultT.SetTValue(CreateToken(user));
        return _resultT;
    }
    
    private async Task<User?> GetUserAsync(IDbConnection connection, string email)
    {
        var sql = "SELECT * FROM Uses WHERE Users.Email = @Email";
        var obj = new { Email = email };
        
        var res = (await connection.QueryAsync<User>(sql, obj)).ToList();
        return res.FirstOrDefault();
    }

    private string CreateToken(User user)
    {
        return _tokenService.CreateSecurityToken(new ClaimsIdentity (new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("Id", user.Id.ToString())
        }), user);
    }
}