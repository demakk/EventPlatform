using Microsoft.Extensions.DependencyInjection;

namespace EventPlatform.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<DataContext>();

        return services;
    }
}