using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{

    public class Transaction
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("balance")]
        public double? Balance { get; set; }

        [JsonProperty("accountId")]
        public Guid AccountId { get; set; }

        [JsonProperty("providerCode")]
        public string ProviderCode { get; set; }
    }
}
