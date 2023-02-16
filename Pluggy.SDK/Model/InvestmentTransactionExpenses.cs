using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class InvestmentTransactionExpenses
    {
        [JsonProperty("serviceTax")]
        public double? ServiceTax { get; set; }

        [JsonProperty("brokerageFee")]
        public double? BrokerageFee { get; set; }

        [JsonProperty("incomeTax")]
        public double? IncomeTax { get; set; }

        [JsonProperty("other")]
        public double? Other { get; set; }

        [JsonProperty("tradingAssetsNoticeFee")]
        public double? TradingAssetsNoticeFee { get; set; }

        [JsonProperty("maintenanceFee")]
        public double? MaintenanceFee { get; set; }

        [JsonProperty("settlementFee")]
        public double? SettlementFee { get; set; }

        [JsonProperty("clearingFee")]
        public double? ClearingFee { get; set; }

        [JsonProperty("stockExchangeFee")]
        public double? StockExchangeFee { get; set; }

        [JsonProperty("custodyFee")]
        public double? CustodyFee { get; set; }

        [JsonProperty("operatingFee")]
        public double? OperatingFee { get; set; }
    }
}
