using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class BoletoMetadata
    {
        [JsonProperty("digitableLine")]
        public string DigitableLine { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("baseAmount")]
        public double? BaseAmount { get; set; }

        [JsonProperty("interestAmount")]
        public double? InterestAmount { get; set; }

        [JsonProperty("penaltyAmount")]
        public double? PenaltyAmount { get; set; }

        [JsonProperty("discountAmount")]
        public double? DiscountAmount { get; set; }
    }
}
