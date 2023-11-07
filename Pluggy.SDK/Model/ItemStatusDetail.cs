using System;
using System.Collections.Generic;
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

        [JsonProperty("warnings")]
        public List<ItemStatusProductDetailWarning> Warnings { get; set; }
    }

    public class ItemStatusProductDetailWarning
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("providerMessage")]
        public string ProviderMessage { get; set; }
    }
}
