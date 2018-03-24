using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class OctopusEvent
    {
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("EventType")]
        public string EventType { get; set; }

        [JsonProperty("Payload")]
        public Payload Payload { get; set; }
    }
}