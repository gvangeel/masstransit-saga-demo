using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.HotelService.Persistence;

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
        var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
        
        if ((await db.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
        {
            //await db.Database.EnsureCreatedAsync(cancellationToken);
            await db.Database.MigrateAsync(cancellationToken);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}