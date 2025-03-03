

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;
public static class Dependencies
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
    {
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
