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
        public CurrencyCode CurrencyCode { get; set; }

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

        [JsonProperty("type")]
        public TransactionType Type { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("paymentData")]
        public TransactionPaymentData PaymentData { get; set; }

        [JsonProperty("status")]
        public TransactionStatus? Status { get; set; }

        [JsonProperty("merchant")]
        public TransactionMerchant? Merchant { get; set; }
    }
}
