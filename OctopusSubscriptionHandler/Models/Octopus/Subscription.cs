using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class Subscription
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}