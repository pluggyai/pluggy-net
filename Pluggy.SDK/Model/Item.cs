using System;
using System.Collections.Generic;
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

        [JsonProperty("clientUserId")]
        public string ClientUserId { get; set; }

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

        [JsonProperty("statusDetail")]
        public ItemStatusDetail StatusDetail { get; set; }

        [JsonProperty("consecutiveFailedLoginAttempts")]
        public int ConsecutiveFailedLoginAttempts { get; set; }

        [JsonProperty("nextAutoSyncAt")]
        public DateTime? NextAutoSyncAt { get; set; }

        [JsonProperty("consentExpiresAt")]
        public DateTime? ConsentExpiresAt { get; set; }

        /// <summary>
        /// Open Finance only. When the institution's resource list was last read for this
        /// item, or null if it never was. An empty page from FetchItemResources means the
        /// institution shared nothing only when this is set.
        /// </summary>
        [JsonProperty("resourcesCollectedAt")]
        public DateTime? ResourcesCollectedAt { get; set; }

        /// <summary>
        /// Open Finance only. True when the institution declares at least one of this item's
        /// resources PENDING_AUTHORISATION: the user still has to approve it at their bank.
        /// False when the resource list was read and none is; null for connectors other than
        /// Open Finance, and while the resource list has not been read yet (ResourcesCollectedAt is null).
        /// </summary>
        [JsonProperty("hasResourcesPendingAuthorization")]
        public bool? HasResourcesPendingAuthorization { get; set; }

        [JsonProperty("products")]
        public IList<ProductType> Products { get; set; }

        [JsonProperty("userAction")]
        public ConnectorUserAction UserAction { get; set; }

        public bool HasFinished()
        {
            return Status == ItemStatus.UPDATED || Status == ItemStatus.OUTDATED || Status == ItemStatus.LOGIN_ERROR;
        }
    }

    public class ConnectorUserAction
    {
        [JsonProperty("instructions")]
        public string Instructions { get; set; }

        [JsonProperty("attributes")]
        public IDictionary<string, object> Attributes { get; set; }

        [JsonProperty("expiresAt")]
        public DateTime? ExpiresAt { get; set; }
    }
}
