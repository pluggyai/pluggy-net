using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class InvestmentInstitution
    {
        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("number")]
        public string? Number { get; set; }
    }
}
