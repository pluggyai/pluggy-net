using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionMerchant
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("businessName")]
        public string BusinessName { get; set; }

        [JsonProperty("cnpj")]
        public string CNPJ { get; set; }

        [JsonProperty("cnae")]
        public string CNAE { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
