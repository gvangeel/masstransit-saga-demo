namespace MassTransit.Saga.Demo.TripService.Configuration.Services;

public static class ConfigureCompositionRoot
{
    public static IServiceCollection AddCompositionRoot(this IServiceCollection services)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        services.AddOptions();
        return services;
    }
}