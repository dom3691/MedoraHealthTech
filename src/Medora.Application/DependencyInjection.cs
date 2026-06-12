using Microsoft.Extensions.DependencyInjection;

namespace Medora.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
