using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Consent
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("products")]
        public List<string> Products { get; set; }

        [JsonProperty("openFinancePermissionsGranted")]
        public List<string> OpenFinancePermissionsGranted { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        [JsonProperty("revokedAt")]
        public DateTime? RevokedAt { get; set; }
    }
}
