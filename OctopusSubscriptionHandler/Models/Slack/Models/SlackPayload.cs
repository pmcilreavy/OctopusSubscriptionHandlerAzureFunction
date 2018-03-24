using System.Collections.Generic;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Slack.Models
{
    public class SlackPayload
    {
        [JsonProperty("channel")]
        public string channel { get; set; }

        [JsonProperty("attachments")]
        public IList<Attachment> attachments { get; set; }
    }
}