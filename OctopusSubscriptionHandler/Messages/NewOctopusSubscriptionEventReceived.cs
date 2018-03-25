using MediatR;
using OctopusSubscriptionHandler.Models.Octopus;

namespace OctopusSubscriptionHandler.Messages
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