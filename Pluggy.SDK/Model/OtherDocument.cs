using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class OtherDocument
    {
        [JsonProperty("type")]
        public OtherDocumentType? Type { get; set; }

        /// <summary>Free-text complement. Populated when Type is OTHER.</summary>
        [JsonProperty("typeAdditionalInfo")]
        public string TypeAdditionalInfo { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("checkDigit")]
        public string CheckDigit { get; set; }

        /// <summary>Free-text complement, used to record the issuing authority (e.g. 'SSP/SP') when relevant.</summary>
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }

    /// <summary>Brazilian document acronyms are kept verbatim; OTHER covers any other type.</summary>
    public enum OtherDocumentType
    {
        CNH,
        RG,
        NIF,
        RNE,
        OTHER
    }
}
