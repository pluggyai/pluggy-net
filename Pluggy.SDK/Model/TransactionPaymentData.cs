using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionPaymentData
    {
        [JsonProperty("payer")]
        public TransactionPaymentParticipant Payer { get; set; }

        [JsonProperty("receiver")]
        public TransactionPaymentParticipant Receiver { get; set; }

        [JsonProperty("paymentMethod")]
        public string PaymentMethod { get; set; }

        [JsonProperty("referenceNumber")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("receiverReferenceId")]
        public string ReceiverReferenceId { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
