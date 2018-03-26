using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Core.Models.Octopus
{
    public class Payload
    {
        [JsonProperty("BatchProcessingDate")]
        public DateTime BatchProcessingDate { get; set; }

        [JsonProperty("Subscription")]
        public Subscription Subscription { get; set; }

        [JsonProperty("Event")]
        public Event Event { get; set; }
    }
}