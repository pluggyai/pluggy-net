using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ItemStatusDetail
    {
        [JsonProperty("accounts")]
        public ItemStatusProductDetail Accounts { get; set; }

        [JsonProperty("creditCards")]
        public ItemStatusProductDetail CreditCards { get; set; }

        [JsonProperty("transactions")]
        public ItemStatusProductDetail Transactions { get; set; }

        [JsonProperty("investments")]
        public ItemStatusProductDetail Investments { get; set; }

        [JsonProperty("investmentsTransactions")]
        public ItemStatusProductDetail InvestmentsTransactions { get; set; }

        [JsonProperty("identity")]
        public ItemStatusProductDetail Identity { get; set; }
    }

    public class ItemStatusProductDetail
    {
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }

        [JsonProperty("lastUpdatedAt")]
        public DateTime? LastUpdatedAt { get; set; }
    }
}
