using MassTransit.Definition;

namespace MassTransit.Saga.Demo.TripService.Application
{
    public class TripSubmissionConsumerDefinition: ConsumerDefinition<TripSubmissionConsumer>
    {
        public TripSubmissionConsumerDefinition()
        {
            EndpointName = "flight-service";
        }
    }
}
