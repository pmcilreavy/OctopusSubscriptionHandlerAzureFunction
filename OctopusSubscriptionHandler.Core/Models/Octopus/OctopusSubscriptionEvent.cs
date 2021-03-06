﻿using System;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Core.Models.Octopus
{
    public class OctopusSubscriptionEvent
    {
        [JsonProperty("Timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("EventType")]
        public string EventType { get; set; }

        [JsonProperty("Payload")]
        public Payload Payload { get; set; }
    }
}