// ReSharper disable InconsistentNaming
namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripSubmissionRejected
    {
        string Reason { get; }
    }
}
