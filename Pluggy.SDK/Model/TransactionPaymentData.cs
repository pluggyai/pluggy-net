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

        /// <summary>
        /// Authentication code of the payment receipt, as printed by the institution on the proof
        /// of payment. It identifies the operation rather than the payment instrument, so it can be
        /// present for any payment method (PIX, TED, DOC, BOLETO).
        /// </summary>
        [JsonProperty("authenticationCode")]
        public string AuthenticationCode { get; set; }

        [JsonProperty("boletoMetadata")]
        public BoletoMetadata BoletoMetadata { get; set; }
    }
}
