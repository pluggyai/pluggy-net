using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class MaritalStatus
    {
        [JsonProperty("code")]
        public MaritalStatusCode Code { get; set; }

        /// <summary>Free-text complement. Populated when Code is OTHER.</summary>
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    public enum MaritalStatusCode
    {
        SINGLE,
        MARRIED,
        WIDOWED,
        JUDICIALLY_SEPARATED,
        DIVORCED,
        STABLE_UNION,
        OTHER
    }
}
