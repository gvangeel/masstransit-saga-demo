using System;

namespace MassTransit.Saga.Demo.TripService.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class FlightBooking
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid FlightId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOutboundFlight { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReturnFlight => !IsOutboundFlight;
        /// <summary>
        /// 
        /// </summary>
        public decimal FlightCost { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string AirlineCompany { get; private set; }

        /// <summary>
        /// 
        /// </summary>
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
}
