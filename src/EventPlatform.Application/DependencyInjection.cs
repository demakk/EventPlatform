using EventPlatform.Application.Commands.IdentityCommandHandlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace EventPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(RegisterUserHandler));
        return services;
    }
}