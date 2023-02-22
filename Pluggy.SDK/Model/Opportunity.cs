using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Opportunity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("totalLimit")]
        public double? TotalLimit { get; set; }

        [JsonProperty("usedLimit")]
        public double? UsedLimit { get; set; }

        [JsonProperty("availableLimit")]
        public double? AvailableLimit { get; set; }

        [JsonProperty("totalQuotas")]
        public int? TotalQuotas { get; set; }

        [JsonProperty("quotasType")]
        public OpportunityDateType? QuotasType { get; set; }

        [JsonProperty("interestRate")]
        public double? InterestRate { get; set; }

        [JsonProperty("rateType")]
        public OpportunityDateType? RateType { get; set; }

        [JsonProperty("type")]
        public OpportunityType Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }
    }
}
