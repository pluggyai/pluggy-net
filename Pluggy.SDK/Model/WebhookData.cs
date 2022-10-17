using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class WebhookData
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}