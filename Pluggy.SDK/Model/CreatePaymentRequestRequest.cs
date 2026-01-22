using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    public class CreatePaymentRequestRequest
    {
        /// <summary>
        /// Payment amount
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// Payment description (optional)
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Payment recipient ID
        /// </summary>
        public Guid RecipientId { get; set; }

        /// <summary>
        /// Customer ID (optional)
        /// </summary>
        public Guid? CustomerId { get; set; }

        /// <summary>
        /// Callback URLs for payment status (optional)
        /// </summary>
        public PaymentCallbackUrls CallbackUrls { get; set; }

        /// <summary>
        /// Client-provided payment ID for reference (optional)
        /// </summary>
        public string ClientPaymentId { get; set; }

        /// <summary>
        /// Whether this is a sandbox payment (optional)
        /// </summary>
        public bool? IsSandbox { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "amount", Amount },
                { "description", Description },
                { "recipientId", RecipientId.ToString() },
                { "customerId", CustomerId?.ToString() },
                { "callbackUrls", CallbackUrls != null ? new Dictionary<string, object>
                    {
                        { "success", CallbackUrls.Success },
                        { "error", CallbackUrls.Error },
                        { "pending", CallbackUrls.Pending }
                    }.RemoveNulls() : null
                },
                { "clientPaymentId", ClientPaymentId },
                { "isSandbox", IsSandbox }
            }.RemoveNulls();
        }
    }
}
