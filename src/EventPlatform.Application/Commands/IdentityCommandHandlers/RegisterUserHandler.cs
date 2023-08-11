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

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, OperationResultT<string>>
{
    private readonly IApplicationDbContext _ctx;
    private readonly ITokenService _tokenService;
    private readonly OperationResultT<string> _result = new();

    public RegisterUserHandler(IApplicationDbContext ctx, ITokenService tokenService)
    {
        _ctx = ctx;
        _tokenService = tokenService;
    }

    public async Task<OperationResultT<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
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

            if (await UserExists(connection, obj))
            {
                _result.AddError(new Error{Code = ErrorCode.UserAlreadyExists, ErrorMessage = "User already exists"});
                return _result;
            }
            
            var sql = "INSERT INTO dbo.Users" +
                      " (Id, FullName, Email, UserName, PasswordHash, CreatedAt, PasswordSalt, Phone)" +
                      " VALUES" +
                      " (@Id, @FullName, @Email, @UserName, @PasswordHash, @CreatedAt, @PasswordSalt, @Phone)";

            var res = await connection.ExecuteAsync(sql, obj);
            _result.SetTValue(CreateToken(obj));
            return _result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _result.AddError(new Error { Code = ErrorCode.ApplicationError, ErrorMessage = e.Message }) ;
            return _result;
        }
    }

    private async Task<bool> UserExists(IDbConnection connection, User user)
    {
        var sql = "SELECT * FROM Users WHERE Users.Email = @Email";
        var obj = new { Email = user.Email };
        
        var res = await connection.QueryAsync<User>(sql, obj);
        return res.FirstOrDefault() is not null;
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