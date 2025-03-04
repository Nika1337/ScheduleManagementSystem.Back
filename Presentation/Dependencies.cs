

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Presentation;
public static class Dependencies
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationJwtBearer(s => s.SigningKey = configuration["JwtSigningKey"]);
        services.AddAuthorization();
        services.AddFastEndpoints();
        services.SwaggerDocument();
        return services;
    }

    public static IApplicationBuilder UsePresentationLayer(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints(c => c.Security.RoleClaimType = ClaimTypes.Role);
        app.UseSwaggerGen();
        return app;
    }
}
