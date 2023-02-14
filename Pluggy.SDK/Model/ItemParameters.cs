using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    public class ItemParameters
    {
        [JsonProperty("connectorId")]
        public long ConnectorId { get; set; }

        [JsonProperty("parameters")]
        public List<ItemParameter> Parameters { get; set; }

        [JsonProperty("webhookUrl")]
        public string WebhookUrl { get; set; }

        [JsonProperty("clientUserId")]
        public string ClientUserId { get; set; }

        public ItemParameters()
        {
        }

        public ItemParameters(long connectorId, List<ItemParameter> credentials, ItemOptions options = null)
        {
            this.Parameters = credentials;
            this.ConnectorId = connectorId;
            if (options != null)
            {
                this.WebhookUrl = options.WebhookUrl;
                this.ClientUserId = options.ClientUserId;
            }
        }

        public IDictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>()
            {
                { "webhookUrl", WebhookUrl },
                { "clientUserId", ClientUserId },
                { "connectorId", ConnectorId },
                { "parameters", Parameters?.ToDictionary(x => x.Name, x => x.Value) },
            }.RemoveNulls();
        }
    }
}
