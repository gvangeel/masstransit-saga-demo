namespace MassTransit.Saga.Demo.TripService.Domain;

public class FlightBooking
{
    public Guid FlightId { get; private set; }
    public bool IsOutboundFlight { get; private set; }
    public bool IsReturnFlight => !IsOutboundFlight;
    public decimal FlightCost { get; private set; }
    public string AirlineCompany { get; private set; }

    private FlightBooking()
    {
        // Required by EFCore
    }

    internal FlightBooking(Guid flightId, bool isOutboundFlight, decimal flightCost, string airlineCompany)
    {
        FlightId = flightId;
        IsOutboundFlight = isOutboundFlight;
        FlightCost = flightCost;
        AirlineCompany = airlineCompany;
    }
}