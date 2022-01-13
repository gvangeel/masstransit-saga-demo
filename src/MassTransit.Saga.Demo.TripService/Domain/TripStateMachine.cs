using Automatonymous;
using GreenPipes;
using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;

namespace MassTransit.Saga.Demo.TripService.Domain;

public class TripStateMachine : MassTransitStateMachine<Trip>
{
    public TripStateMachine(ILogger<TripStateMachine> logger)
    {
        InstanceState(_ => _.CurrentState);

        Event(() => TripRegistrationRequest, x => x.CorrelateById(c => c.Message.TripId));
        Event(() => FlightBooked, x => x.CorrelateById(c => c.Message.TripId));
        Event(() => HotelBooked, x => x.CorrelateById(c => c.Message.TripId));
        Event(() => TripCancellationRequest, x => x.CorrelateById(c => c.Message.TripId));
        Event(() => TripStateRequest, x =>
        {
            x.CorrelateById(c => c.Message.TripId);
            x.OnMissingInstance(m => m.ExecuteAsync(context => context.RespondAsync<ITripNotFound>(new
            {
                context.Message.TripId
            })));
        });

        Initially(
            When(TripRegistrationRequest)
                .ThenAsync(Initialize)
                .TransitionTo(PendingFlightBookingConfirmations));

        During(PendingFlightBookingConfirmations,
            When(FlightBooked)
                .Then(context => context.Instance.Handle(context.Data))
                .IfElse(context => context.Instance.AllFlightsBooked, 
                    thenBinder => thenBinder
                        .ThenAsync(HandleAllFlightsBooked)
                        .TransitionTo(PendingHotelBookingConfirmation), 
                    elseBinder => elseBinder.Then(
                        _ =>
                        {
                            logger.LogInformation("Received a flight booking confirmation, but not all flights confirmed yet");
                        })));

        During(PendingHotelBookingConfirmation,
            When(HotelBooked)
                .Then(context => context.Instance.Handle(context.Data))
                .TransitionTo(Completed)
            //.Finalize()
        );
            
        DuringAny(
            When(TripCancellationRequest)
                .Then(_ =>
                {
                    logger.LogWarning("TODO: handle a TripCancellationRequestReceived");
                })
                .TransitionTo(Cancelled));

        DuringAny(
            When(TripStateRequest)
                .RespondAsync(x => x.Init<ITripState>(new
                {
                    TripId = x.Instance.CorrelationId,
                    InVar.Timestamp,
                    State = x.Instance.CurrentState,
                    x.Instance.OutboundFlightBooked,
                    x.Instance.ReturnFlightBooked,
                    x.Instance.HotelBooked
                })));

        //SetCompletedWhenFinalized();
    }

    public State PendingFlightBookingConfirmations { get; private set; }
    public State PendingHotelBookingConfirmation { get; private set; }
    public State Cancelled { get; private set; }
    public State Completed { get; private set; }
    public Event<ITripRegistrationRequest> TripRegistrationRequest { get; private set; }
    public Event<IFlightBooked> FlightBooked { get; private set; }
    public Event<IHotelBooked> HotelBooked { get; private set; }
    public Event<ITripCancellationRequest> TripCancellationRequest { get; private set; }
    public Event<ITripStateRequest> TripStateRequest { get; private set; }

    private static async Task Initialize(BehaviorContext<Trip, ITripRegistrationRequest> context)
    {
        context.Instance.Initialize(context.Data);
        var consumeContext = context.GetPayload<ConsumeContext>();
        await consumeContext.Publish<ITripRegistered>(new {
            TripId = context.Instance.CorrelationId,
            context.Instance.RequiredStars,
            context.Instance.Destination,
            context.Instance.Start,
            context.Instance.End
        });
    }

    private static async Task HandleAllFlightsBooked(BehaviorContext<Trip, IFlightBooked> context)
    {
        var consumeContext = context.GetPayload<ConsumeContext>();
        await consumeContext.Publish<ITripAllFlightsBooked>(new
        {
            TripId = context.Instance.CorrelationId,
        });
    }
}