using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    #region Requests

    /// <summary>
    /// Request to create an Automatic Pix payment request (recurring Pix consent).
    /// </summary>
    public class CreateAutomaticPixPaymentRequest
    {
        /// <summary>Fixed charge amount. If set, minimumVariableAmount/maximumVariableAmount cannot be provided.</summary>
        public double? FixedAmount { get; set; }

        /// <summary>Minimum amount allowed per charge (variable amount consent).</summary>
        public double? MinimumVariableAmount { get; set; }

        /// <summary>Maximum amount allowed per charge (variable amount consent).</summary>
        public double? MaximumVariableAmount { get; set; }

        /// <summary>Description for the automatic pix authorization.</summary>
        public string Description { get; set; }

        /// <summary>Expected date for the first occurrence (YYYY-MM-DD).</summary>
        public string StartDate { get; set; }

        /// <summary>Expiration date for the authorization (YYYY-MM-DDTHH:MM:SSZ).</summary>
        public string ExpiresAt { get; set; }

        /// <summary>Whether the receiving customer is allowed to make payment attempts.</summary>
        public bool? IsRetryAccepted { get; set; }

        /// <summary>Recipient ID for the consent.</summary>
        public Guid? RecipientId { get; set; }

        /// <summary>Customer ID associated to the consent.</summary>
        public Guid? CustomerId { get; set; }

        /// <summary>Callback URLs for the consent flow.</summary>
        public PaymentCallbackUrls CallbackUrls { get; set; }

        /// <summary>Definitions for the first (enrollment) payment.</summary>
        public AutomaticPixFirstPayment FirstPayment { get; set; }

        /// <summary>Permitted frequency for recurring payments.</summary>
        public AutomaticPixInterval? Interval { get; set; }

        /// <summary>Configuration for automatic retries.</summary>
        public AutomaticPixRetriesConfiguration AutomaticRetriesConfiguration { get; set; }

        /// <summary>Whether this is a sandbox payment.</summary>
        public bool? IsSandbox { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "fixedAmount", FixedAmount },
                { "minimumVariableAmount", MinimumVariableAmount },
                { "maximumVariableAmount", MaximumVariableAmount },
                { "description", Description },
                { "startDate", StartDate },
                { "expiresAt", ExpiresAt },
                { "isRetryAccepted", IsRetryAccepted },
                { "recipientId", RecipientId?.ToString() },
                { "customerId", CustomerId?.ToString() },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error },
                        { "pending", CallbackUrls.Pending }
                    }.RemoveNulls() : null
                },
                { "firstPayment", FirstPayment != null ? new Dictionary<string, object>
                    {
                        { "amount", FirstPayment.Amount },
                        { "date", FirstPayment.Date },
                        { "description", FirstPayment.Description }
                    }.RemoveNulls() : null
                },
                { "interval", Interval?.ToString() },
                { "automaticRetriesConfiguration", AutomaticRetriesConfiguration != null ? new Dictionary<string, object>
                    {
                        { "retryDays", AutomaticRetriesConfiguration.RetryDays }
                    }.RemoveNulls() : null
                },
                { "isSandbox", IsSandbox }
            }.RemoveNulls();
        }
    }

    /// <summary>
    /// Request to schedule an Automatic Pix payment under an existing consent.
    /// </summary>
    public class ScheduleAutomaticPixPaymentRequest
    {
        /// <summary>Transaction value.</summary>
        public double Amount { get; set; }

        /// <summary>Transaction description (optional).</summary>
        public string Description { get; set; }

        /// <summary>Payment date (D+2 to D+10), format YYYY-MM-DD.</summary>
        public string Date { get; set; }

        /// <summary>External identifier for the payment (optional).</summary>
        public string ClientPaymentId { get; set; }

        /// <summary>Recipient ID (optional, must share the consented recipient's tax number).</summary>
        public Guid? RecipientId { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "amount", Amount },
                { "description", Description },
                { "date", Date },
                { "clientPaymentId", ClientPaymentId },
                { "recipientId", RecipientId?.ToString() }
            }.RemoveNulls();
        }
    }

    /// <summary>
    /// Request to retry an Automatic Pix payment.
    /// </summary>
    public class RetryAutomaticPixPaymentRequest
    {
        /// <summary>Date to retry the payment within a 7-day window (YYYY-MM-DD).</summary>
        public string Date { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "date", Date }
            }.RemoveNulls();
        }
    }

    #endregion

    #region Responses

    /// <summary>
    /// An individual recurring (Automatic) Pix payment.
    /// </summary>
    public class AutomaticPixPayment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public AutomaticPixPaymentStatus Status { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("endToEndId")]
        public string EndToEndId { get; set; }

        [JsonProperty("errorDetail")]
        public AutomaticPixPaymentErrorDetail ErrorDetail { get; set; }

        [JsonProperty("clientPaymentId")]
        public string ClientPaymentId { get; set; }

        [JsonProperty("recipientId")]
        public Guid? RecipientId { get; set; }

        [JsonProperty("isFirstPayment")]
        public bool? IsFirstPayment { get; set; }

        [JsonProperty("attempts")]
        public List<AutomaticPixPaymentAttempt> Attempts { get; set; }

        /// <summary>
        /// Only present on the single-payment detail endpoint. Indicates whether the payment
        /// reached the scheduled status in the last attempt (useful to determine retry eligibility).
        /// </summary>
        [JsonProperty("scheduledStatusReached")]
        public bool? ScheduledStatusReached { get; set; }
    }

    public class AutomaticPixFirstPayment
    {
        /// <summary>Target settlement date of the first payment. If not provided, settled immediately.</summary>
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }

    public class AutomaticPixRetriesConfiguration
    {
        /// <summary>Days after the original payment date on which scheduled payments may be retried (1-7).</summary>
        [JsonProperty("retryDays")]
        public List<int> RetryDays { get; set; }
    }

    public class AutomaticPixPaymentErrorDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    public class AutomaticPixPaymentAttempt
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("endToEndId")]
        public string EndToEndId { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("errorDetail")]
        public AutomaticPixPaymentErrorDetail ErrorDetail { get; set; }
    }

    #endregion

    #region Enums

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum AutomaticPixInterval
    {
        WEEKLY,
        MONTHLY,
        QUARTERLY,
        SEMESTER,
        YEARLY
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum AutomaticPixPaymentStatus
    {
        SCHEDULED,
        CREATED,
        COMPLETED,
        CANCELED,
        ERROR
    }

    #endregion
}
