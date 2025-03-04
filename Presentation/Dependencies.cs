

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;
public static class Dependencies
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthenticationJwtBearer(s => s.SigningKey = configuration["JwtSigningKey"]);
        services.AddAuthorization();
        services.AddFastEndpoints();
        return services;
    }

    public static IApplicationBuilder UsePresentationLayer(this IApplicationBuilder app)
    {
        app.UseFastEndpoints();
        app.UseSwaggerGen();
        return app;
    }
}
