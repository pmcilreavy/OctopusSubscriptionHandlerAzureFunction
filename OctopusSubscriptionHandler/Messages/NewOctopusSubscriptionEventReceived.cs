using System;
using System.Net;
using MediatR;
using Microsoft.Azure.WebJobs.Host;
using OctopusSubscriptionHandler.Models.Octopus;

namespace OctopusSubscriptionHandler.Messages
{
    public class NewOctopusSubscriptionEventReceived : INotification
    {
        public IPAddress ClientIp { get; }
        public Guid RequestId { get; }
        public OctopusEvent SubscriptionEvent { get; }
        public TraceWriter Logger { get; }

        public NewOctopusSubscriptionEventReceived(Guid requestId, IPAddress clientIp, OctopusEvent subscriptionEvent, TraceWriter logger)
        {
            ClientIp = clientIp;
            RequestId = requestId;
            SubscriptionEvent = subscriptionEvent;
            Logger = logger;
        }
    }
}