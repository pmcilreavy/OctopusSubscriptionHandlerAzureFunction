using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OctopusSubscriptionHandler.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            var json = "{\"Timestamp\":\"2018-03-23T01:55:07.4595727+00:00\",\"EventType\":\"SubscriptionPayload\",\"Payload\":{\"ServerUri\":null,\"ServerAuditUri\":null,\"BatchProcessingDate\":\"2018-03-23T11:55:06.1783313+10:00\",\"Subscription\":{\"Id\":\"Subscriptions-1\",\"Name\":\"Notify Readify of Deployments\",\"Type\":0,\"IsDisabled\":false,\"EventNotificationSubscription\":{\"Filter\":{\"Users\":[],\"Projects\":[\"Projects-286\",\"Projects-161\",\"Projects-23\",\"Projects-5\",\"Projects-61\",\"Projects-12\",\"Projects-13\",\"Projects-241\",\"Projects-7\",\"Projects-81\",\"Projects-3\",\"Projects-301\",\"Projects-8\",\"Projects-140\",\"Projects-201\",\"Projects-25\",\"Projects-41\",\"Projects-462\"],\"Environments\":[],\"EventGroups\":[],\"EventCategories\":[],\"Tenants\":[],\"Tags\":[],\"DocumentTypes\":[]},\"EmailTeams\":[],\"EmailFrequencyPeriod\":\"01:00:00\",\"EmailPriority\":1,\"EmailDigestLastProcessed\":null,\"EmailDigestLastProcessedEventAutoId\":null,\"EmailShowDatesInTimeZoneId\":\"E. Australia Standard Time\",\"WebhookURI\":\"http://webhook.site/469a397c-71ce-4825-b482-a96ef8b4c31a\",\"WebhookTeams\":[],\"WebhookTimeout\":\"00:00:10\",\"WebhookHeaderKey\":null,\"WebhookHeaderValue\":null,\"WebhookLastProcessed\":\"2018-03-23T11:54:05.7407094+10:00\",\"WebhookLastProcessedEventAutoId\":105598},\"Links\":{\"Self\":{}}},\"Event\":{\"Id\":\"Events-105948\",\"RelatedDocumentIds\":[\"Deployments-39013\",\"Projects-462\",\"Releases-28332\",\"Environments-2\",\"ServerTasks-98618\",\"Channels-344\",\"ProjectGroups-1\"],\"Category\":\"DeploymentStarted\",\"UserId\":\"users-system\",\"Username\":\"system\",\"IdentityEstablishedWith\":\"Unknown\",\"Occurred\":\"2018-03-23T01:54:33.4595039+00:00\",\"Message\":\"Deploy to Test started  for test-proj release 0.0.7 to Test\",\"MessageHtml\":\"hhh\",\"MessageReferences\":[{\"ReferencedDocumentId\":\"Deployments-39013\",\"StartIndex\":0,\"Length\":14},{\"ReferencedDocumentId\":\"Projects-462\",\"StartIndex\":28,\"Length\":9},{\"ReferencedDocumentId\":\"Releases-28332\",\"StartIndex\":46,\"Length\":5},{\"ReferencedDocumentId\":\"Environments-2\",\"StartIndex\":55,\"Length\":4}],\"Comments\":null,\"Details\":null,\"Links\":{\"Self\":{}}},\"BatchId\":\"b863b593-8f89-430f-9437-00b9319fde9d\",\"TotalEventsInBatch\":3,\"EventNumberInBnBatch\":2}}";


            // TODO
        }
    }
}
