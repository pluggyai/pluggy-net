using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionPaymentParticipant
    {
        [JsonProperty("documentNumber")]
        public Document DocumentNumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("branchNumber")]
        public string BranchNumber { get; set; }

        [JsonProperty("routingNumber")]
        public string RoutingNumber { get; set; }

        [JsonProperty("routingNumberISPB")]
        public string RoutingNumberISPB { get; set; }
    }
}
