using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Underlying debtor of receivables-backed paper (CRI / CRA).
    /// </summary>
    public class InvestmentDebtor
    {
        /// <summary>Name of the underlying debtor.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
