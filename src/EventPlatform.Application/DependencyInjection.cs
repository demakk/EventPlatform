using System.Text;
using EventPlatform.Application.Commands.IdentityCommandHandlers;
using EventPlatform.Application.Commands.IdentityCommands;
using EventPlatform.Application.Interfaces;
using EventPlatform.Application.Options;
using EventPlatform.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;


namespace EventPlatform.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddTransient<JwtSettings>();
        services.AddMediatR(typeof(RegisterUserCommand));
        services.AddTransient<ITokenService, TokenService>();

        services.Configure<JwtSettings>(config.GetSection(nameof(JwtSettings)));
        
        var jwtSettings = new JwtSettings();
        config.Bind(nameof(JwtSettings), jwtSettings);
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = false,
                    //ValidAudiences = jwtSettings.Audience,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
                //jwt.Audience = jwtSettings.Audience[0];
                jwt.ClaimsIssuer = jwtSettings.Issuer;
            });

        
        return services;
    }
}