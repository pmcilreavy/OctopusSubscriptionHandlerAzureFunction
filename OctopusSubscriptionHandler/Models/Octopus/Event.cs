using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class Event
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("RelatedDocumentIds")]
        public IList<string> RelatedDocumentIds { get; set; }

        [JsonProperty("Category")]
        public string Category { get; set; }

        [JsonProperty("UserId")]
        public string UserId { get; set; }

        [JsonProperty("Username")]
        public string Username { get; set; }

        [JsonProperty("IdentityEstablishedWith")]
        public string IdentityEstablishedWith { get; set; }

        [JsonProperty("Occurred")]
        public DateTime Occurred { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("MessageHtml")]
        public string MessageHtml { get; set; }

        [JsonProperty("MessageReferences")]
        public IList<MessageReference> MessageReferences { get; set; }
    }
}