using System;
using System.Linq;
using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.TripService.Domain;
using PeanutButter.DuckTyping.Extensions;
using Xunit;

namespace MassTransit.Saga.Demo.TripService.Tests.Domain
{
    public class TripShould
    {
        [Fact]
        public void BeInitializedFromATripRequest()
        {
            var tripId = NewId.NextGuid();
            var triprequest = new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "test",
                Start = new DateTime(2022, 2, 1),
                End = new DateTime(2022, 2, 3)
            };

            var sut = new Trip();
            sut.Initialize(triprequest.DuckAs<ITripRegistrationRequest>());

            Assert.Equal(5, sut.RequiredStars);
            Assert.Equal("test", sut.Destination);
            Assert.Equal(new DateTime(2022, 2, 1), sut.Start);
            Assert.Equal(new DateTime(2022, 2, 3), sut.End);
        }

        [Fact]
        public void ValidateFalseWhenRequiredStarsNotInRange()
        {
            var tripId = NewId.NextGuid();
            var triprequest = new
            {
                TripId = tripId,
                RequiredStars = 10,
                Destination = "test",
                Start = new DateTime(2022, 2, 1),
                End = new DateTime(2022, 2, 3)
            };

            var actual = Trip.Validate(triprequest.DuckAs<ISubmitTrip>());
            Assert.True(actual.IsFailure);
            Assert.Equal("Required stars should be between 1 and 5", actual.Error);
        }

        [Fact]
        public void ValidateFalseWhenDatesNotInOrder()
        {
            var tripId = NewId.NextGuid();
            var triprequest = new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "test",
                Start = new DateTime(2022, 2, 1),
                End = new DateTime(2022, 1, 1)
            };

            var actual = Trip.Validate(triprequest.DuckAs<ISubmitTrip>());
            Assert.True(actual.IsFailure);
            Assert.Equal("End date should be after start date", actual.Error);
        }

        [Fact]
        public void ValidateFalseWhenEmptyDescription()
        {
            var tripId = NewId.NextGuid();
            var triprequest = new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "",
                Start = new DateTime(2022, 2, 1),
                End = new DateTime(2022, 2, 3)
            };

            var actual = Trip.Validate(triprequest.DuckAs<ISubmitTrip>());
            Assert.True(actual.IsFailure);
            Assert.Equal("Destination is required", actual.Error);
        }

        [Fact]
        public void ValidateTrueWhenRequirementsAreMet()
        {
            var tripId = NewId.NextGuid();
            var triprequest = new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "test",
                Start = new DateTime(2022, 2, 1),
                End = new DateTime(2022, 2, 3)
            };

            var actual = Trip.Validate(triprequest.DuckAs<ISubmitTrip>());
            Assert.True(actual.IsSuccess);
        }

        [Fact]
        public void HandleAFlightBooking()
        {
            var tripId = NewId.NextGuid();
            var flightId = NewId.NextGuid();
            var flightBooked = new
            {
                TripId = tripId,
                IsOutbound = false,
                FlightId = flightId,
                Cost = 1.00,
                Company = "Company"
            };
            var sut = new Trip();
            var flight = flightBooked.ForceFuzzyDuckAs<IFlightBooked>();
            sut.Handle(flight);

            Assert.Equal(1, sut.BookedFlights.Count);
            var bookedFlight = sut.BookedFlights.FirstOrDefault()!;
            Assert.Equal(flightId, bookedFlight.FlightId);
            Assert.False(bookedFlight.IsOutboundFlight);
            Assert.True(bookedFlight.IsReturnFlight);
            Assert.Equal("Company", bookedFlight.AirlineCompany);
            Assert.Equal((decimal)1.00, bookedFlight.FlightCost);
        }

        [Fact]
        public void HandleAHotelBooking()
        {
            var tripId = NewId.NextGuid();
            var hotelBookingId = NewId.NextGuid();
            var hotelBooked = new
            {
                TripId = tripId,
                Stars = 5,
                HotelName = "hotel",
                HotelBookingId = hotelBookingId
            };
            var sut = new Trip();
            var bookedHotel = hotelBooked.ForceFuzzyDuckAs<IHotelBooked>();
            sut.Handle(bookedHotel);

            Assert.NotNull(sut.HotelBooking);
            Assert.Equal("hotel", sut.HotelBooking?.HotelName);
            Assert.Equal(5, sut.HotelBooking?.HotelStars);
            Assert.Equal(hotelBookingId, sut.HotelBooking?.HotelBookingId);

        }
    }
}
