using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Item
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("connector")]
        public Connector Connector { get; set; }

        [JsonProperty("status")]
        public ItemStatus Status { get; set; }

        [JsonProperty("executionStatus")]
        public string ExecutionStatus { get; set; }

        [JsonProperty("webhookUrl")]
        public string WebhookUrl { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("lastUpdatedAt")]
        public DateTime? LastUpdatedAt { get; set; }

        [JsonProperty("error")]
        public ExecutionError Error { get; set; }

        [JsonProperty("parameter")]
        public ConnectorParameter Parameter { get; set; }


        public bool HasFinished()
        {
            return Status == ItemStatus.UPDATED || Status == ItemStatus.OUTDATED || Status == ItemStatus.LOGIN_ERROR;
        }
    }
}
