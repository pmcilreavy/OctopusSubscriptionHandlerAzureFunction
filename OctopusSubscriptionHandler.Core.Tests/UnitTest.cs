using System;
using OctopusSubscriptionHandler.Core.MessageHandlers;
using OctopusSubscriptionHandler.Core.Messages;
using OctopusSubscriptionHandler.Core.Models.Octopus;
using Xunit;

namespace OctopusSubscriptionHandler.Core.Tests
{
    public class TestIncomingOctopusSubscriptionEventRequestHandler
    {
        [Fact]
        public void TestMethod()
        {
            var evt = new OctopusSubscriptionEvent
            {
                EventType = "SubscriptionPayload",
                Timestamp = DateTime.Now,
                Payload = new Payload
                {
                    BatchProcessingDate = DateTime.Now,
                    Event = new Event
                    {
                        Category = "DeploymedSucceeded",
                        Message = "Project blah was promoted to Prod",
                        Occurred = DateTime.Now,
                        Username = "system"
                    },
                    Subscription = new Subscription { Name = "Notify peeps about deployments" }
                }
            };

            var o = new NewOctopusSubscriptionEventReceived(evt);

            var slackHandler = new SlackMessageOnNewOctopusSubscriptionEventReceivedHandler();
            //var t = slackHandler.Handle(o, CancellationToken.None);
        }
    }
}
