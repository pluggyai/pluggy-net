using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class PhoneNumber
    {
        [JsonProperty("type")]
        public PhoneNumberType? Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>International dialing code (DDI). Populated when different from '55'.</summary>
        [JsonProperty("countryCallingCode")]
        public string CountryCallingCode { get; set; }

        /// <summary>Area code (DDD) of the phone.</summary>
        [JsonProperty("areaCode")]
        public string AreaCode { get; set; }

        /// <summary>Extension number, when part of the phone identification.</summary>
        [JsonProperty("extension")]
        public string Extension { get; set; }

        /// <summary>Additional info related to the source phone type.</summary>
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    public enum PhoneNumberType
    {
        Personal,
        Work,
        Residencial
    }
}
