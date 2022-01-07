using MassTransit.Saga.Demo.TripService.Domain;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.Demo.TripService.Tests.Domain;

public class TripSagaFixture: IDisposable
{
    public InMemoryTestHarness Harness { get; }
    public IStateMachineSagaTestHarness<Trip, TripStateMachine> SagaHarness { get; }
    public TripSagaFixture()
    {
        var provider = new ServiceCollection()
            .AddLogging()
            .AddMassTransitInMemoryTestHarness(config =>
            {
                config.AddSagaStateMachine<TripStateMachine, Trip>(sagaConfig =>
                    {
                        //sagaConfig.UseConcurrentMessageLimit(1);
                        sagaConfig.UseInMemoryOutbox();
                    })
                    .InMemoryRepository();

                config.AddSagaStateMachineTestHarness<TripStateMachine, Trip>();
            }).BuildServiceProvider(true);

        Harness = provider.GetRequiredService<InMemoryTestHarness>();
        Harness.OnConfigureInMemoryReceiveEndpoint += config =>
        {
            config.ConfigureSagas(provider.GetRequiredService<IBusRegistrationContext>());
        };

        SagaHarness = provider.GetRequiredService<IStateMachineSagaTestHarness<Trip, TripStateMachine>>();
    }

    public void Dispose()
    {
        Harness.Dispose();
        GC.SuppressFinalize(this);
    }
}