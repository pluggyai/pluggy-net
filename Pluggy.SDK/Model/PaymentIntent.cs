using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    public class PaymentIntent
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("paymentRequestId")]
        public Guid PaymentRequestId { get; set; }

        [JsonProperty("paymentRequest")]
        public PaymentRequest PaymentRequest { get; set; }

        [JsonProperty("connectorId")]
        public long? ConnectorId { get; set; }

        [JsonProperty("connector")]
        public Connector Connector { get; set; }

        [JsonProperty("status")]
        public PaymentIntentStatus Status { get; set; }

        [JsonProperty("consentUrl")]
        public string ConsentUrl { get; set; }

        [JsonProperty("parameters")]
        public Dictionary<string, string> Parameters { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PaymentIntentStatus
    {
        CONSENT_AWAITING_AUTHORIZATION,
        CONSENT_AUTHORIZED,
        CONSENT_REJECTED,
        PAYMENT_PENDING,
        PAYMENT_PARTIALLY_ACCEPTED,
        PAYMENT_SETTLEMENT_PROCESSING,
        PAYMENT_SETTLEMENT_DEBTOR_ACCOUNT,
        PAYMENT_COMPLETED,
        PAYMENT_REJECTED,
        PAYMENT_ERROR,
        ERROR
    }
}
