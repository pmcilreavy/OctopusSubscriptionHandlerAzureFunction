using System.Collections.Generic;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Core.Models.Slack
{
    public class SlackPayload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("attachments")]
        public IList<Attachment> Attachments { get; set; }
    }
}