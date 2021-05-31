using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ItemOptions
    {
        [JsonProperty("webhookUrl")]
        public string WebhookUrl { get; set; }

        [JsonProperty("clientUserId")]
        public string ClientUserId { get; set; }

        public ItemOptions()
        {

        }
    }
}
