using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ReservedBalance
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("identification")]
        public string Identification { get; set; }

        [JsonProperty("availableAmounts")]
        public ICollection<ReservedBalanceAmount> AvailableAmounts { get; set; }
    }

    public class ReservedBalanceAmount
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("remuneration", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReservedBalanceRemuneration Remuneration { get; set; }
    }

    public class ReservedBalanceRemuneration
    {
        [JsonProperty("preFixedRate")]
        public double? PreFixedRate { get; set; }

        [JsonProperty("postFixedIndexerPercentage")]
        public double? PostFixedIndexerPercentage { get; set; }

        [JsonProperty("rateType")]
        public string RateType { get; set; }

        [JsonProperty("indexer")]
        public string Indexer { get; set; }

        [JsonProperty("calculation")]
        public string Calculation { get; set; }

        [JsonProperty("ratePeriodicity")]
        public string RatePeriodicity { get; set; }

        [JsonProperty("indexerAdditionalInfo")]
        public string IndexerAdditionalInfo { get; set; }
    }
}
