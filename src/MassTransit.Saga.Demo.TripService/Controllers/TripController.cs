using MassTransit.Saga.Demo.Contracts.Trips;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.Saga.Demo.TripService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripController : ControllerBase
{
    private readonly IRequestClient<ISubmitTrip> _submitTripClient;
    private readonly IRequestClient<ITripStateRequest> _tripStateRequestClient;
    private readonly IPublishEndpoint _publishEndpoint;

    public TripController(
        IRequestClient<ISubmitTrip> submitTripClient,
        IRequestClient<ITripStateRequest> tripStateRequestClient,
        IPublishEndpoint publishEndpoint)
    {
        _submitTripClient = submitTripClient;
        _tripStateRequestClient = tripStateRequestClient;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Book([FromQuery] int requiredStars, [FromQuery] string destination, [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var tripId = NewId.NextGuid();
        var (accepted, rejected) = await _submitTripClient.GetResponse<ITripSubmissionAccepted, ITripSubmissionRejected>(new
        {
            TripId = tripId,
            RequiredStars = requiredStars,
            Destination = destination,
            Start = start,
            End = end
        });

        if (accepted.IsCompletedSuccessfully)
        {
            await accepted;
            return AcceptedAtRoute("GetTripState", new { Id = tripId}, new { Id = tripId } );
        }

        var response = await rejected;
        return BadRequest(response.Message.Reason);
    }

    [HttpGet("{id}", Name = "GetTripState")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var (trip, notFound) = await _tripStateRequestClient.GetResponse<ITripState, ITripNotFound>(new
        {
            TripId = id
        });

        if (trip.IsCompletedSuccessfully)
        {
            var response = await trip;
            return Ok(response.Message);
        }

        await notFound;
        return NotFound(id);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromQuery] string reason)
    {
        await _publishEndpoint.Publish<ITripCancellationRequest>(new
        {
            TripId = id,
            Reason = reason
        });

        return Accepted();
    }
}