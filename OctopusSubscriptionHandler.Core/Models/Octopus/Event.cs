using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Core.Models.Octopus
{
    public class Event
    {
        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("Occurred")]
        public DateTime Occurred { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }
    }
}