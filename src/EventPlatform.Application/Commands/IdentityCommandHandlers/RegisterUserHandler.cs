using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using EventPlatform.Application.Commands.IdentityCommands;
using EventPlatform.Application.Interfaces;
using EventPlatform.Domain.Models;
using MediatR;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventPlatform.Application.Commands.IdentityCommandHandlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly IApplicationDbContext _ctx;
    private readonly ITokenService _tokenService;

    public RegisterUserHandler(IApplicationDbContext ctx, ITokenService tokenService)
    {
        _ctx = ctx;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var connection = _ctx.CreateConnection();
            connection.Open();
        
            using var hmac = new HMACSHA512();
            var obj = new User
            {
                Id = Guid.NewGuid(), FullName = request.FullName, Email = request.Email, UserName = request.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
                PasswordSalt = hmac.Key, CreatedAt = DateTime.Now, Phone = request.Phone
            };

            var sql = "INSERT INTO dbo.Users" +
                      " (Id, FullName, Email, UserName, PasswordHash, CreatedAt, PasswordSalt, Phone)" +
                      " VALUES" +
                      " (@Id, @FullName, @Email, @UserName, @PasswordHash, @CreatedAt, @PasswordSalt, @Phone)";
        
            var res = await connection.ExecuteAsync(sql, obj);
            return CreateToken(obj);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    private async Task<bool> UserExists(IDbConnection connection, User user)
    {
        var sql = "SELECT * FROM Users WHERE Users.Id = @UserId";
        var obj = new { UserId = user.Id };
        
        var res = await connection.QueryAsync<User>(sql, obj);
        return res.First() is null;
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