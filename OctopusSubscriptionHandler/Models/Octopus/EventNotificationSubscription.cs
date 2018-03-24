using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class EventNotificationSubscription
    {
        [JsonProperty("Filter")]
        public Filter Filter { get; set; }

        [JsonProperty("EmailFrequencyPeriod")]
        public string EmailFrequencyPeriod { get; set; }

        [JsonProperty("EmailPriority")]
        public int EmailPriority { get; set; }

        [JsonProperty("EmailShowDatesInTimeZoneId")]
        public string EmailShowDatesInTimeZoneId { get; set; }

        [JsonProperty("WebhookURI")]
        public string WebhookURI { get; set; }

        [JsonProperty("WebhookTimeout")]
        public string WebhookTimeout { get; set; }

        [JsonProperty("WebhookLastProcessed")]
        public DateTime WebhookLastProcessed { get; set; }

        [JsonProperty("WebhookLastProcessedEventAutoId")]
        public int WebhookLastProcessedEventAutoId { get; set; }
    }
}