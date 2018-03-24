using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class Payload
    {
        [JsonProperty("ServerUri")]
        public object ServerUri { get; set; }

        [JsonProperty("ServerAuditUri")]
        public object ServerAuditUri { get; set; }

        [JsonProperty("BatchProcessingDate")]
        public DateTime BatchProcessingDate { get; set; }

        [JsonProperty("Subscription")]
        public Subscription Subscription { get; set; }

        [JsonProperty("Event")]
        public Event Event { get; set; }

        [JsonProperty("BatchId")]
        public string BatchId { get; set; }

        [JsonProperty("TotalEventsInBatch")]
        public int TotalEventsInBatch { get; set; }

        [JsonProperty("EventNumberInBatch")]
        public int EventNumberInBatch { get; set; }
    }
}