using EventPlatform.Application.Commands.IdentityCommands;
using MediatR;

namespace EventPlatform.Application.Commands.IdentityCommandHandlers;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
{
    
    public Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}