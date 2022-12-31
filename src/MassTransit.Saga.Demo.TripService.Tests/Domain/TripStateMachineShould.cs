using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.TripService.Domain;
using MassTransit.Testing;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Assert = Xunit.Assert;

namespace MassTransit.Saga.Demo.TripService.Tests.Domain;
// https://masstransit-project.com/usage/testing.html#test-setup

public class TripStateMachineShould
{
    [Fact]
    public async Task CreateANewTripInstance()
    {
        var machine = new TripStateMachine(NullLogger<TripStateMachine>.Instance);
        var fixture = new TripSagaFixture();
        await fixture.Harness.Start();
        try
        {
            var tripId = NewId.NextGuid();

            await fixture.Harness.Bus.Publish<ITripRegistrationRequest>(new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "test",
                Start = new DateTime(2022, 1, 1),
                End = new DateTime(2022, 2, 1)
            });

            // did the endpoint consume the message
            Assert.True(fixture.Harness.Consumed.Select<ITripRegistrationRequest>().Any());

            // did the actual consumer consume the message
            Assert.True(fixture.Harness.Consumed.Select<ITripRegistrationRequest>().Any());

            // did the saga get created
            Assert.True(fixture.SagaHarness.Created.Select(x => x.CorrelationId == tripId).Any());

            // is the saga in the correct state
            Assert.NotNull(fixture.SagaHarness.Sagas.ContainsInState(tripId, machine, machine.PendingFlightBookingConfirmations));

            // verify the saga's properties are set correctly
            var instance = fixture.SagaHarness.Created.Select(x => x.CorrelationId == tripId).First();
            Assert.Equal("PendingFlightBookingConfirmations", instance.Saga.CurrentState);
            Assert.Equal(5, instance.Saga.RequiredStars);
            Assert.Equal(new DateTime(2022, 1, 1), instance.Saga.Start);
            Assert.Equal(new DateTime(2022, 2, 1), instance.Saga.End);
            Assert.Empty(instance.Saga.BookedFlights);
            Assert.Null(instance.Saga.HotelBooking);

            //Did the 2 flight request get published
            Assert.Equal(2, await fixture.Harness.Published.SelectAsync<IBookFlightRequest>().Count());
        }
        finally
        {
            await fixture.Harness.Stop();
        }
    }

    [Fact]
    public async Task FulllyProcessATripRequest()
    {
        var machine = new TripStateMachine(NullLogger<TripStateMachine>.Instance);
        var fixture = new TripSagaFixture();

        await fixture.Harness.Start();
        try
        {
            var tripId = NewId.NextGuid();

            //Create a TripRequest
            var message = new
            {
                TripId = tripId,
                RequiredStars = 5,
                Destination = "test",
                Start = new DateTime(2022, 1, 1),
                End = new DateTime(2022, 2, 1)
            };
            await fixture.Harness.Bus.Publish<ITripRegistrationRequest>(message);
            Assert.True(await fixture.Harness.Consumed.Any<ITripRegistrationRequest>());
            Assert.True(fixture.SagaHarness.Created.Select(x => x.CorrelationId == tripId).Any());
            Assert.NotNull(fixture.SagaHarness.Sagas.ContainsInState(tripId,machine, machine.PendingFlightBookingConfirmations));

            Assert.Equal(2, await fixture.Harness.Published.SelectAsync<IBookFlightRequest>().Count());

            //Book both flights
            var outBoundFlightBooked = new
            {
                TripId = tripId,
                IsOutbound = false,
                FlightId = NewId.NextGuid(),
                Cost = (decimal)1.00,
                Company = "someCompany"
            };
            var inBoundFlightBooked = new
            {
                TripId = tripId,
                IsOutbound = true,
                FlightId = NewId.NextGuid(),
                Cost = (decimal)1.00,
                Company = "someCompany"
            };
            await fixture.Harness.Bus.Publish<IFlightBooked>(outBoundFlightBooked);
            await fixture.Harness.Bus.Publish<IFlightBooked>(inBoundFlightBooked);
            await Task.Delay(500);

            Assert.Equal(2, await fixture.Harness.Consumed.SelectAsync<IFlightBooked>().Count());
            Assert.Equal(1, await fixture.Harness.Published.SelectAsync<IBookHotelRequest>().Count());
            Assert.NotNull(fixture.SagaHarness.Sagas.ContainsInState(tripId, machine, machine.PendingHotelBookingConfirmation));

            //Book hotel
            await fixture.Harness.Bus.Publish<IHotelBooked>(new
            {
                TripId = tripId,
                Stars = 5,
                HotelName = "someHotel",
                HotelBookingId = NewId.NextGuid()
            });
            await Task.Delay(10);
            
            Assert.NotNull(fixture.SagaHarness.Created.ContainsInState(tripId, machine, machine.Completed));
            Assert.True(await fixture.Harness.Consumed.SelectAsync<IHotelBooked>().Any());
        }
        finally
        {
            await fixture.Harness.Stop();
        }
    }
}
