namespace MassTransit.Saga.Demo.TransportationService.Domain;

public class FlightBooking
{
    public Guid FlightId { get; private set; }
    public bool IsOutboundFlight { get; private set; }
    public bool IsReturnFlight => !IsOutboundFlight;
    public decimal FlightCost { get; private set; }
    public string AirlineCompany { get; private set; }
    public Guid TripId { get; private set; }
    public string From { get; private set; }
    public string To { get; private set; }
    public DateTime FlightDate { get; private set; }

    private FlightBooking()
    {
        // Required by EFCore
    }

    internal FlightBooking(Guid flightId, bool isOutboundFlight, decimal flightCost, string airlineCompany, Guid tripId, string from, string to, DateTime flightDate)
    {
        FlightId = flightId;
        IsOutboundFlight = isOutboundFlight;
        FlightCost = flightCost;
        AirlineCompany = airlineCompany;
        TripId = tripId;
        From = from;
        To = to;
        FlightDate = flightDate;
    }
}