using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    #region Requests

    /// <summary>
    /// Request to create a Pix por biometria payment request.
    /// Pix por biometria has no sandbox, so there is no IsSandbox option.
    /// </summary>
    public class CreatePixBiometricPaymentRequest
    {
        /// <summary>Amount in BRL, with up to two decimal places (minimum 0.01).</summary>
        public double Amount { get; set; }

        /// <summary>Recipient that receives the payment. Bank account recipients only.</summary>
        public Guid RecipientId { get; set; }

        /// <summary>Shown to the payer (optional, up to 140 characters).</summary>
        public string Description { get; set; }

        /// <summary>Payment Customer that identifies the payer (optional). Without it the payer types their CPF.</summary>
        public Guid? CustomerId { get; set; }

        /// <summary>Your own identifier for the payment (optional, up to 200 characters).</summary>
        public string ClientPaymentId { get; set; }

        /// <summary>Callback URLs for payment status (optional).</summary>
        public PaymentCallbackUrls CallbackUrls { get; set; }

        /// <summary>Offer the payer to also share their account balance and overdraft limit while the link lasts (optional).</summary>
        public bool? LinkedJourney { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "amount", Amount },
                { "recipientId", RecipientId.ToString() },
                { "description", Description },
                { "customerId", CustomerId?.ToString() },
                { "clientPaymentId", ClientPaymentId },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error },
                        { "pending", CallbackUrls.Pending }
                    }.RemoveNulls() : null
                },
                { "linkedJourney", LinkedJourney }
            }.RemoveNulls();
        }
    }

    /// <summary>Filters for listing Pix por biometria enrollments.</summary>
    public class PixBiometricEnrollmentParameters
    {
        /// <summary>Filter by Payment Customer.</summary>
        public Guid? CustomerId { get; set; }

        /// <summary>Filter by payer CPF. Digits only: the enrollment returns it masked, so a returned value cannot be passed back.</summary>
        public string Cpf { get; set; }

        /// <summary>Filter by payer CNPJ. Digits only: the enrollment returns it masked, so a returned value cannot be passed back.</summary>
        public string Cnpj { get; set; }

        /// <summary>Filter by enrollment status.</summary>
        public PixBiometricEnrollmentStatus? Status { get; set; }

        public int? Page { get; set; }

        public int? PageSize { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            var queryStrings = new Dictionary<string, string>();

            if (CustomerId.HasValue) queryStrings.Add("customerId", CustomerId.Value.ToString());
            if (!string.IsNullOrEmpty(Cpf)) queryStrings.Add("cpf", Cpf);
            if (!string.IsNullOrEmpty(Cnpj)) queryStrings.Add("cnpj", Cnpj);
            if (Status.HasValue) queryStrings.Add("status", Status.Value.ToString());
            if (Page.HasValue) queryStrings.Add("page", Page.Value.ToString());
            if (PageSize.HasValue) queryStrings.Add("pageSize", PageSize.Value.ToString());

            return queryStrings;
        }
    }

    /// <summary>Filters for listing payment intents.</summary>
    public class PaymentIntentParameters
    {
        /// <summary>Filter by payment request.</summary>
        public Guid? PaymentRequestId { get; set; }

        /// <summary>Filter by Pix por biometria enrollment (linked account).</summary>
        public Guid? EnrollmentId { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            var queryStrings = new Dictionary<string, string>();

            if (PaymentRequestId.HasValue) queryStrings.Add("paymentRequestId", PaymentRequestId.Value.ToString());
            if (EnrollmentId.HasValue) queryStrings.Add("enrollmentId", EnrollmentId.Value.ToString());

            return queryStrings;
        }
    }

    #endregion

    #region Responses

    /// <summary>
    /// Institution where a Pix por biometria enrollment is held.
    /// </summary>
    public class PixBiometricEnrollmentConnector
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }

    /// <summary>
    /// The linked account, as the institution reported it.
    /// </summary>
    public class PixBiometricDebtor
    {
        /// <summary>Account holder name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>Account holder CPF/CNPJ, masked.</summary>
        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("bankAccount")]
        public PixBiometricBankAccount BankAccount { get; set; }
    }

    public class PixBiometricBankAccount
    {
        /// <summary>Branch number.</summary>
        [JsonProperty("agency")]
        public string Agency { get; set; }

        /// <summary>Account number, masked.</summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>Institution name.</summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Short form of an enrollment, embedded in a payment request.
    /// </summary>
    public class PixBiometricEnrollmentSummary
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public PixBiometricEnrollmentStatus Status { get; set; }

        /// <summary>Institution where the account is held.</summary>
        [JsonProperty("connector")]
        public PixBiometricEnrollmentConnector Connector { get; set; }

        /// <summary>The linked account. Null until the institution reports it.</summary>
        [JsonProperty("debtor")]
        public PixBiometricDebtor Debtor { get; set; }
    }

    /// <summary>
    /// A linked account (enrollment): the payer's account at one institution, linked to the device
    /// and browser where they created the passkey. Only the payer creates one, on the payment page.
    /// </summary>
    public class PixBiometricEnrollment : PixBiometricEnrollmentSummary
    {
        /// <summary>Payer's CPF, masked.</summary>
        [JsonProperty("cpf")]
        public string Cpf { get; set; }

        /// <summary>Payer's CNPJ, masked.</summary>
        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }

        /// <summary>Payment Customer of the payment request where the link was created. Null when the payer typed their CPF.</summary>
        [JsonProperty("customerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>Limits the payer set at the bank, in BRL. Null until the institution reports them.</summary>
        [JsonProperty("limits")]
        public PixBiometricEnrollmentLimits Limits { get; set; }

        /// <summary>When the link expires, as set by the institution.</summary>
        [JsonProperty("expiresAt")]
        public DateTime? ExpiresAt { get; set; }

        /// <summary>Balance-sharing permission. Null when the payment request did not offer it or the payer chose not to share.</summary>
        [JsonProperty("dataConsent")]
        public PixBiometricDataConsent DataConsent { get; set; }

        /// <summary>Who revoked the link. Null unless Status is REVOKED.</summary>
        [JsonProperty("revokedBy")]
        public PixBiometricRevokedBy? RevokedBy { get; set; }

        /// <summary>Reason the institution gave when the link was rejected.</summary>
        [JsonProperty("errorDetail")]
        public PixBiometricErrorDetail ErrorDetail { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PixBiometricEnrollmentLimits
    {
        /// <summary>Per-payment limit, in BRL.</summary>
        [JsonProperty("transaction")]
        public decimal? Transaction { get; set; }

        /// <summary>Daily limit, in BRL.</summary>
        [JsonProperty("daily")]
        public decimal? Daily { get; set; }

        /// <summary>Part of the daily limit already used, in BRL.</summary>
        [JsonProperty("dailyUsed")]
        public decimal? DailyUsed { get; set; }
    }

    public class PixBiometricDataConsent
    {
        [JsonProperty("status")]
        public PixBiometricDataConsentStatus Status { get; set; }

        /// <summary>Why the permission was rejected; null otherwise.</summary>
        [JsonProperty("rejectionReason")]
        public string RejectionReason { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PixBiometricErrorDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Pix por biometria details of a payment request; null on every other kind of payment request.
    /// </summary>
    public class PaymentRequestPixBiometric
    {
        /// <summary>Whether the payer was offered balance sharing.</summary>
        [JsonProperty("linkedJourney")]
        public bool LinkedJourney { get; set; }

        /// <summary>The linked account the request was paid with. Null until the request is paid.</summary>
        [JsonProperty("enrollment")]
        public PixBiometricEnrollmentSummary Enrollment { get; set; }
    }

    public class PixBiometricBalance
    {
        /// <summary>Account balance in BRL.</summary>
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        /// <summary>Overdraft limit of the account; null when the institution does not share it.</summary>
        [JsonProperty("overdraft")]
        public SmartTransferOverdraft Overdraft { get; set; }
    }

    /// <summary>
    /// Payload of the pix_biometric_enrollment/* webhook events.
    /// The "id" field is not sent for these events; use EventId.
    /// </summary>
    public class PixBiometricWebhookEventPayload
    {
        [JsonProperty("eventId")]
        public Guid EventId { get; set; }

        [JsonProperty("event")]
        private readonly string _event;

        public WebhookEvent Event
        {
            get { return new WebhookEvent(_event); }
        }

        /// <summary>Primary identifier of the enrollment (linked account).</summary>
        [JsonProperty("enrollmentId")]
        public Guid EnrollmentId { get; set; }

        /// <summary>Payment Customer of the payment request where the link was created; null when the payer typed their CPF.</summary>
        [JsonProperty("customerId")]
        public Guid? CustomerId { get; set; }

        /// <summary>Primary identifier of the payment request where the link was created.</summary>
        [JsonProperty("paymentRequestId")]
        public Guid PaymentRequestId { get; set; }
    }

    #endregion

    #region Enums

    /// <summary>
    /// CREATED: the payer started linking and has not finished. AUTHORIZED: the link is active.
    /// REJECTED: the link was not completed. REVOKED: revoked at the bank or through the API.
    /// EXPIRED: the link reached its expiration date or became inactive at the institution.
    /// REJECTED, REVOKED and EXPIRED are final.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PixBiometricEnrollmentStatus
    {
        CREATED,
        AUTHORIZED,
        REJECTED,
        REVOKED,
        EXPIRED
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PixBiometricDataConsentStatus
    {
        AWAITING_AUTHORISATION,
        AUTHORISED,
        REJECTED,
        REVOKED
    }

    /// <summary>
    /// BANK: the payer or the institution, at the bank. CLIENT: you, through the API.
    /// PAYER: older links only.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PixBiometricRevokedBy
    {
        BANK,
        CLIENT,
        PAYER
    }

    /// <summary>How a payment intent is paid.</summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PaymentMethod
    {
        PIS,
        PIX,
        PIX_BIOMETRIC
    }

    #endregion
}
