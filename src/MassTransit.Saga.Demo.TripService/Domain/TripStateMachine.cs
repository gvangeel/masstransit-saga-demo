using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;
using Microsoft.Extensions.Logging;

namespace MassTransit.Saga.Demo.TripService.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class TripStateMachine : MassTransitStateMachine<Trip>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
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
        /// <summary>
        /// 
        /// </summary>
        public State PendingFlightBookingConfirmations { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public State PendingHotelBookingConfirmation { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public State Cancelled { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public State Completed { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Event<ITripRegistrationRequest> TripRegistrationRequest { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Event<IFlightBooked> FlightBooked { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Event<IHotelBooked> HotelBooked { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Event<ITripCancellationRequest> TripCancellationRequest { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public Event<ITripStateRequest> TripStateRequest { get; private set; }
        /// <summary>
        /// 
        /// </summary>

        private static async Task Initialize(BehaviorContext<Trip, ITripRegistrationRequest> context)
        {
            context.Instance.Initialize(context.Data);
            var consumeContext = context.GetPayload<ConsumeContext>();
            await consumeContext.Publish<IBookFlightRequest>(new
            {
                TripId = context.Instance.CorrelationId,
                DayOfFlight = context.Instance.Start,
                IsOutbound = true,
                From = "home",
                To = context.Instance.Destination
            });

            await consumeContext.Publish<IBookFlightRequest>(new
            {
                TripId = context.Instance.CorrelationId,
                DayOfFlight = context.Instance.End,
                IsOutbound = false,
                From = context.Instance.Destination,
                To = "home"
            });
        }

        private static async Task HandleAllFlightsBooked(BehaviorContext<Trip, IFlightBooked> context)
        {
            var consumeContext = context.GetPayload<ConsumeContext>();
            await consumeContext.Publish<IBookHotelRequest>(new
            {
                TripId = context.Instance.CorrelationId,
                context.Instance.RequiredStars,
                Location = context.Instance.Destination
            });
        }
    }
}
