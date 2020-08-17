using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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

        public ItemParameters()
        {
        }

        public ItemParameters(long connectorId, List<ItemParameter> credentials, string webhookUrl = "")
        {
            this.Parameters = credentials;
            this.WebhookUrl = webhookUrl;
            this.ConnectorId = connectorId;
        }

        public IDictionary<string, string> ToBody()
        {
            return new Dictionary<string, string>(Parameters.ToDictionary(x => x.Name, x => x.Value))
            {
                { "webhookUrl", WebhookUrl },
                { "connectorId", ConnectorId.ToString() }
            };
        }
    }
}
