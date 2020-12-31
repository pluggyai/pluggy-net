using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Connector
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("institutionUrl")]
        public string InstitutionUrl { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonProperty("type")]
        public ConnectorType Type { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("credentials")]
        public IList<ConnectorParameter> Credentials { get; set; }
    }
}
