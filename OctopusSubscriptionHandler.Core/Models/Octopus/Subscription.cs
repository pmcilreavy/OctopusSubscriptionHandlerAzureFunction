using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Core.Models.Octopus
{
    public class Subscription
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}