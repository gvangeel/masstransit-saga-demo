using System.Threading.Tasks;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.TripService.Domain;

namespace MassTransit.Saga.Demo.TripService.Application
{
    /// <summary>
    /// 
    /// </summary>
    public class TripSubmissionConsumer : IConsumer<ISubmitTrip>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<ISubmitTrip> context)
        {
            var validationResult = Trip.Validate(context.Message);
            if (validationResult.IsFailure)
            {
                await context.RespondAsync<ITripSubmissionRejected>(new {Reason = validationResult.Error});
            }
            else
            {
                await context.Publish<ITripRegistrationRequest>(new
                {
                    context.Message.TripId,
                    context.Message.RequiredStars,
                    context.Message.Destination,
                    context.Message.Start,
                    context.Message.End
                });

                await context.RespondAsync<ITripSubmissionAccepted>(new { });
            }
        }
    }
}
