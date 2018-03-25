using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OctopusSubscriptionHandler.Messages;
using Serilog;

namespace OctopusSubscriptionHandler.MessageHandlers
{
    public class LogMessageOnNewOctopusSubscriptionEventReceivedHandler : INotificationHandler<NewOctopusSubscriptionEventReceived>
    {
        public Task Handle(NewOctopusSubscriptionEventReceived notification, CancellationToken cancellationToken)
        {
            Log.Information(notification.SubscriptionEventEvent.Payload.Event.Message);

            return Task.CompletedTask;
        }
    }
}