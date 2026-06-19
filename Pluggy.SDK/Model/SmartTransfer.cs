using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    #region Requests

    /// <summary>
    /// Request to create a Smart Transfer payment under an existing preauthorization.
    /// </summary>
    public class CreateSmartTransferPayment
    {
        public string PreauthorizationId { get; set; }

        public string RecipientId { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public string ClientPaymentId { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "preauthorizationId", PreauthorizationId },
                { "recipientId", RecipientId },
                { "amount", Amount },
                { "description", Description },
                { "clientPaymentId", ClientPaymentId }
            }.RemoveNulls();
        }
    }

    /// <summary>
    /// Request to create a Smart Transfer preauthorization (recurring transfer consent).
    /// </summary>
    public class CreateSmartTransferPreauthorization
    {
        public long ConnectorId { get; set; }

        /// <summary>Payer identification (CPF required, CNPJ optional).</summary>
        public SmartTransferPreauthorizationParameter Parameters { get; set; }

        public List<string> RecipientIds { get; set; }

        public SmartTransferCallbackUrls CallbackUrls { get; set; }

        public string ClientPreauthorizationId { get; set; }

        public SmartTransferPreauthorizationConfiguration Configuration { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "connectorId", ConnectorId },
                { "parameters", Parameters != null ? new Dictionary<string, object>
                    {
                        { "cpf", Parameters.Cpf },
                        { "cnpj", Parameters.Cnpj }
                    }.RemoveNulls() : null
                },
                { "recipientIds", RecipientIds != null ? RecipientIds.Cast<object>().ToList() : null },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error }
                    }.RemoveNulls() : null
                },
                { "clientPreauthorizationId", ClientPreauthorizationId },
                { "configuration", Configuration != null ? Configuration.ToBody() : null }
            }.RemoveNulls();
        }
    }

    #endregion

    #region Responses

    /// <summary>
    /// A Smart Transfer payment.
    /// </summary>
    public class SmartTransferPayment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("preauthorizationId")]
        public string PreauthorizationId { get; set; }

        [JsonProperty("status")]
        public SmartTransferPaymentStatus Status { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("recipient")]
        public PaymentRecipient Recipient { get; set; }

        [JsonProperty("clientPaymentId")]
        public string ClientPaymentId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("errorDetail")]
        public SmartTransferErrorDetail ErrorDetail { get; set; }
    }

    /// <summary>
    /// A Smart Transfer preauthorization (recurring transfer consent).
    /// </summary>
    public class SmartTransferPreauthorization
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public SmartTransferPreauthorizationStatus Status { get; set; }

        [JsonProperty("consentUrl")]
        public string ConsentUrl { get; set; }

        [JsonProperty("clientPreauthorizationId")]
        public string ClientPreauthorizationId { get; set; }

        [JsonProperty("callbackUrls")]
        public SmartTransferCallbackUrls CallbackUrls { get; set; }

        [JsonProperty("recipients")]
        public List<PaymentRecipient> Recipients { get; set; }

        [JsonProperty("connector")]
        public Connector Connector { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("configuration")]
        public SmartTransferPreauthorizationConfiguration Configuration { get; set; }

        [JsonProperty("errorDetail")]
        public SmartTransferErrorDetail ErrorDetail { get; set; }
    }

    public class SmartTransferErrorDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    #endregion

    #region Shared

    public class SmartTransferCallbackUrls
    {
        [JsonProperty("success")]
        public string Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public class SmartTransferPreauthorizationParameter
    {
        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }
    }

    public class SmartTransferPreauthorizationConfiguration
    {
        /// <summary>Maximum amount reachable by the sum of all transactions under this consent.</summary>
        [JsonProperty("totalAllowedAmount")]
        public double? TotalAllowedAmount { get; set; }

        /// <summary>Maximum amount for each payment transaction.</summary>
        [JsonProperty("transactionLimit")]
        public double? TransactionLimit { get; set; }

        [JsonProperty("periodicLimits")]
        public SmartTransferPeriodicLimits PeriodicLimits { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "totalAllowedAmount", TotalAllowedAmount },
                { "transactionLimit", TransactionLimit },
                { "periodicLimits", PeriodicLimits != null ? new Dictionary<string, object>
                    {
                        { "day", PeriodicLimits.Day?.ToBody() },
                        { "week", PeriodicLimits.Week?.ToBody() },
                        { "month", PeriodicLimits.Month?.ToBody() },
                        { "year", PeriodicLimits.Year?.ToBody() }
                    }.RemoveNulls() : null
                }
            }.RemoveNulls();
        }
    }

    public class SmartTransferPeriodicLimits
    {
        [JsonProperty("day")]
        public SmartTransferPeriodicLimit Day { get; set; }

        [JsonProperty("week")]
        public SmartTransferPeriodicLimit Week { get; set; }

        [JsonProperty("month")]
        public SmartTransferPeriodicLimit Month { get; set; }

        [JsonProperty("year")]
        public SmartTransferPeriodicLimit Year { get; set; }
    }

    public class SmartTransferPeriodicLimit
    {
        /// <summary>Maximum number of transactions allowed in the period.</summary>
        [JsonProperty("quantityLimit")]
        public double? QuantityLimit { get; set; }

        /// <summary>Maximum amount to be transacted in the period.</summary>
        [JsonProperty("transactionLimit")]
        public double? TransactionLimit { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "quantityLimit", QuantityLimit },
                { "transactionLimit", TransactionLimit }
            }.RemoveNulls();
        }
    }

    #endregion

    #region Enums

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum SmartTransferPaymentStatus
    {
        CONSENT_AUTHORIZED,
        CONSENT_REJECTED,
        PAYMENT_PENDING,
        PAYMENT_PARTIALLY_ACCEPTED,
        PAYMENT_SETTLEMENT_PROCESSING,
        PAYMENT_SETTLEMENT_DEBTOR_ACCOUNT,
        PAYMENT_COMPLETED,
        PAYMENT_REJECTED,
        ERROR
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum SmartTransferPreauthorizationStatus
    {
        CREATED,
        COMPLETED,
        REVOKED,
        REJECTED,
        ERROR
    }

    #endregion
}
