using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Merchant (company) information, looked up by CNPJ.
    /// </summary>
    public class Merchant
    {
        /// <summary>Merchant name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Merchant legal business name.</summary>
        [JsonProperty("businessName")]
        public string BusinessName { get; set; }

        /// <summary>Document number (CNPJ) related to the merchant.</summary>
        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        /// <summary>Economic activity classification (CNAE) number related to the merchant.</summary>
        [JsonProperty("cnae")]
        public string Cnae { get; set; }
    }

    /// <summary>
    /// Result of a merchant lookup by CNPJ list (GET /merchants).
    /// </summary>
    public class GetMerchantsResponse
    {
        /// <summary>Merchants found for the provided CNPJs.</summary>
        [JsonProperty("foundMerchants")]
        public List<Merchant> FoundMerchants { get; set; }

        /// <summary>Valid CNPJs that were not found in the merchant database.</summary>
        [JsonProperty("notFoundMerchants")]
        public List<string> NotFoundMerchants { get; set; }

        /// <summary>Invalid CNPJ values provided.</summary>
        [JsonProperty("invalidCnpjs")]
        public List<string> InvalidCnpjs { get; set; }
    }
}
