using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Webhook
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

    }
}
