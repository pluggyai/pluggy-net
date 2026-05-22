using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Additional document for businesses headquartered abroad and not required to register a CNPJ.
    /// </summary>
    public class BusinessOtherDocument
    {
        /// <summary>Type of the document (e.g. 'EIN').</summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>Issuing country in alpha3 ISO-3166 format.</summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }
}
