using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class Subscription
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        public int Type { get; set; }

        [JsonProperty("IsDisabled")]
        public bool IsDisabled { get; set; }

        [JsonProperty("EventNotificationSubscription")]
        public EventNotificationSubscription EventNotificationSubscription { get; set; }
    }
}