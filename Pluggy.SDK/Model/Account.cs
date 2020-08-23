using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{

    public enum Type { BANK, CREDIT }

    public enum Subtype { SAVINGS_ACCOUNT, CHECKINGS_ACCOUNT, CREDIT_CARD }

    public class Account
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("marketingName")]
        public string MarketingName { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("accountType")]
        public string Type { get; set; }

        [JsonProperty("accountSubtype")]
        public string Subtype { get; set; }
    }
}
