using MassTransit.Saga.Demo.TripService.Persistence;

namespace MassTransit.Saga.Demo.TripService.Infrastructure;

public class EfDbCreatedHostedService : IHostedService
{
    readonly IServiceProvider _serviceProvider;

    public EfDbCreatedHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TripDbContext>();
        await db.Database.EnsureCreatedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}