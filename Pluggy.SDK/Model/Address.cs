using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Address
    {
        [JsonProperty("type")]
        public AddressType? Type { get; set; }

        [JsonProperty("fullAddress")]
        public string FullAddress { get; set; }

        [JsonProperty("primaryAddress")]
        public string PrimaryAddress { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }

        /// <summary>District / neighborhood (bairro).</summary>
        [JsonProperty("district")]
        public string District { get; set; }

        /// <summary>IBGE municipality code (7 digits). The first two digits identify the Federation Unit.</summary>
        [JsonProperty("ibgeTownCode")]
        public string IbgeTownCode { get; set; }

        /// <summary>Country code in alpha3 ISO-3166 format (e.g. 'BRA').</summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        /// <summary>Geographic coordinates of the address.</summary>
        [JsonProperty("geographicCoordinates")]
        public GeographicCoordinates GeographicCoordinates { get; set; }
    }

    public enum AddressType
    {
        Personal,
        Work
    }
}
