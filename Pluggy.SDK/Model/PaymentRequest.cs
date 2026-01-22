using System;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    public class PaymentRequest
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("recipientId")]
        public Guid RecipientId { get; set; }

        [JsonProperty("recipient")]
        public PaymentRecipient Recipient { get; set; }

        [JsonProperty("customerId")]
        public Guid? CustomerId { get; set; }

        [JsonProperty("customer")]
        public PaymentCustomer Customer { get; set; }

        [JsonProperty("status")]
        public PaymentRequestStatus Status { get; set; }

        [JsonProperty("paymentUrl")]
        public string PaymentUrl { get; set; }

        [JsonProperty("callbackUrls")]
        public PaymentCallbackUrls CallbackUrls { get; set; }

        [JsonProperty("clientPaymentId")]
        public string ClientPaymentId { get; set; }

        [JsonProperty("isSandbox")]
        public bool? IsSandbox { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentCallbackUrls
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("pending")]
        public string Pending { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PaymentRequestStatus
    {
        CREATED,
        IN_PROGRESS,
        COMPLETED,
        ERROR,
        CANCELLED
    }
}
