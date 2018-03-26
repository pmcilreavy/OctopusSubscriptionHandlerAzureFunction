using MediatR;
using OctopusSubscriptionHandler.Core.Models.Octopus;

namespace OctopusSubscriptionHandler.Core.Messages
{
    public class NewOctopusSubscriptionEventReceived : INotification
    {
        public OctopusSubscriptionEvent SubscriptionEventEvent { get; }

        public NewOctopusSubscriptionEventReceived(OctopusSubscriptionEvent subscriptionEventEvent)
        {
            SubscriptionEventEvent = subscriptionEventEvent;
        }
    }
}