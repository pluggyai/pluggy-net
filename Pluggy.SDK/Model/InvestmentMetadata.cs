using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class InvestmentMetadata
    {
        [JsonProperty("taxRegime")]
        public string TaxRegime { get; set; }

        [JsonProperty("proposalNumber")]
        public string ProposalNumber { get; set; }

        [JsonProperty("processNumber")]
        public string ProcessNumber { get; set; }
    }
}
