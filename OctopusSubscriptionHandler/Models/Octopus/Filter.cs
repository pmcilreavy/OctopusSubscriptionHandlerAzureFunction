using System.Collections.Generic;
using Newtonsoft.Json;

namespace OctopusSubscriptionHandler.Models.Octopus
{
    public class Filter
    {
        [JsonProperty("Projects")]
        public IList<string> Projects { get; set; }
    }
}