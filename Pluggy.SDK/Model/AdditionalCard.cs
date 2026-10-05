using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// An additional credit card associated with a credit card account.
    /// </summary>
    public class AdditionalCard
    {
        /// <summary>Number of the additional credit card.</summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
