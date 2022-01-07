using Automatonymous;
using CSharpFunctionalExtensions;
using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;

namespace MassTransit.Saga.Demo.TripService.Domain;

public class Trip : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; private set; }
    public int RequiredStars { get; private set; }
    public string Destination { get; private set; }
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private readonly List<FlightBooking> _bookedFlights = new();
    public IReadOnlyList<FlightBooking> BookedFlights => _bookedFlights.AsReadOnly();
    public bool HotelBooked => HotelBooking != null;
    public HotelBooking? HotelBooking { get; private set; }
    public bool OutboundFlightBooked => _bookedFlights.Exists(_ => _.IsOutboundFlight);
    public bool ReturnFlightBooked => _bookedFlights.Exists(_ => _.IsReturnFlight);
    public bool AllFlightsBooked => OutboundFlightBooked && ReturnFlightBooked;

    public static Result Validate(ISubmitTrip message) =>
        Result.Combine(
            Result.FailureIf(message.Start >= message.End,
                "End date should be after start date"),
            Result.FailureIf(message.RequiredStars <= 0 || message.RequiredStars > 5,
                "Required stars should be between 1 and 5"),
            Result.FailureIf(string.IsNullOrWhiteSpace(message.Destination),
                "Destination is required"));

    public void Initialize(ITripRegistrationRequest message)
    {
        RequiredStars = message.RequiredStars;
        Destination = message.Destination;
        Start = message.Start;
        End = message.End;
    }

    public void Handle(IFlightBooked message)
    {
        var booking = new FlightBooking(message.FlightId, message.IsOutbound, message.Cost, message.Company);
        _bookedFlights.Add(booking);
    }

    public void Handle(IHotelBooked message)
    {
        HotelBooking = new HotelBooking(message.HotelBookingId, message.Stars, message.HotelName);
    }
}