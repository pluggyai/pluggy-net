using System;
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

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("credentials")]
        public IList<ConnectorParameter> Credentials { get; set; }
    }
}
