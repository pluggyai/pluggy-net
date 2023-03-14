using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionCreditCardMetadata
    {
        [JsonProperty("installmentNumber")]
        public int? InstallmentNumber { get; set; }

        [JsonProperty("totalInstallments")]
        public int? TotalInstallments { get; set; }

        [JsonProperty("totalAmount")]
        public double? TotalAmount { get; set; }
    }
}

