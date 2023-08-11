using EventPlatform.Domain.Common;
using MediatR;

namespace EventPlatform.Application.Commands.IdentityCommands;

public class RegisterUserCommand: IRequest<OperationResultT<string>>
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
}