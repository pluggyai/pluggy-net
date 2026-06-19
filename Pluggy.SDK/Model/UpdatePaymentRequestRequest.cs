using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Request to update an existing payment request (PATCH /payments/requests/{id}).
    /// All fields are optional; only provided fields are updated.
    /// </summary>
    public class UpdatePaymentRequestRequest
    {
        public double? Amount { get; set; }

        public string Description { get; set; }

        public PaymentCallbackUrls CallbackUrls { get; set; }

        public Guid? RecipientId { get; set; }

        public Guid? CustomerId { get; set; }

        public string ClientPaymentId { get; set; }

        public bool? IsSandbox { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "amount", Amount },
                { "description", Description },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error },
                        { "pending", CallbackUrls.Pending }
                    }.RemoveNulls() : null
                },
                { "recipientId", RecipientId?.ToString() },
                { "customerId", CustomerId?.ToString() },
                { "clientPaymentId", ClientPaymentId },
                { "isSandbox", IsSandbox }
            }.RemoveNulls();
        }
    }

    /// <summary>
    /// Request to create a payment request from a Pix QR code (POST /payments/requests/pix-qr).
    /// </summary>
    public class CreatePixQrPaymentRequest
    {
        /// <summary>Pix QR code (copia e cola / EMV string).</summary>
        public string PixQrCode { get; set; }

        public PaymentCallbackUrls CallbackUrls { get; set; }

        public Guid? CustomerId { get; set; }

        public bool? IsSandbox { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "pixQrCode", PixQrCode },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error },
                        { "pending", CallbackUrls.Pending }
                    }.RemoveNulls() : null
                },
                { "customerId", CustomerId?.ToString() },
                { "isSandbox", IsSandbox }
            }.RemoveNulls();
        }
    }
}
