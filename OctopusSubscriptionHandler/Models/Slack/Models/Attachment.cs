using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Slack.Models
{
    public class Attachment
    {
        [JsonProperty("fallback")]
        public string fallback { get; set; }

        [JsonProperty("color")]
        public string color { get; set; }

        [JsonProperty("pretext")]
        public string pretext { get; set; }

        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("footer")]
        public string footer { get; set; }

        [JsonProperty("footer_icon")]
        public string footer_icon { get; set; }

        [JsonProperty("ts")]
        public long ts { get; set; }
    }
}