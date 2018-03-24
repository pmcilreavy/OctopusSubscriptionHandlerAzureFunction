using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OctopusSubscriptionHandler.Messages;

namespace OctopusSubscriptionHandler.MessageHandlers
{
    public class LogMessageOnNewOctopusSubscriptionEventReceivedHandler : INotificationHandler<NewOctopusSubscriptionEventReceived>
    {
        public Task Handle(NewOctopusSubscriptionEventReceived notification, CancellationToken cancellationToken)
        {
            notification.Logger.Info(notification.SubscriptionEvent.Payload.Event.Message);

            return Task.CompletedTask;
        }
    }
}