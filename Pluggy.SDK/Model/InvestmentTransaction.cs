using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class InvestmentTransaction
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public InvestmentTransactionType Type { get; set; }

        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("investmentId")]
        public string InvestmentId { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("value")]
        public double? Value { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("tradeDate")]
        public DateTime TradeDate { get; set; }

        [JsonProperty("brokerageNumber")]
        public string BrokerageNumber { get; set; }

        [JsonProperty("netAmount")]
        public double? NetAmount { get; set; }

        [JsonProperty("expenses")]
        public InvestmentTransactionExpenses Expenses { get; set; }
    }
}
