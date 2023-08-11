using EventPlatform.Domain.Common;
using MediatR;

namespace EventPlatform.Application.Commands.IdentityCommands;

public class LoginUserCommand : IRequest<OperationResultT<string>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}