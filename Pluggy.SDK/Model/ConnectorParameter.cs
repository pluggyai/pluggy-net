using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ConnectorParameter
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("mfa")]
        public Boolean Mfa { get; set; }

    }
}
