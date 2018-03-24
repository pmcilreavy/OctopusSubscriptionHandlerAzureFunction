using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class MessageReference
    {
        [JsonProperty("ReferencedDocumentId")]
        public string ReferencedDocumentId { get; set; }

        [JsonProperty("StartIndex")]
        public int StartIndex { get; set; }

        [JsonProperty("Length")]
        public int Length { get; set; }
    }
}