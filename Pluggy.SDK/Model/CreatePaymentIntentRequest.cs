using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    public class CreatePaymentIntentRequest
    {
        /// <summary>
        /// Payment request ID to pay
        /// </summary>
        public Guid PaymentRequestId { get; set; }

        /// <summary>
        /// Connector ID (financial institution)
        /// </summary>
        public long ConnectorId { get; set; }

        /// <summary>
        /// Parameters required by the connector (e.g., cpf, cnpj)
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "paymentRequestId", PaymentRequestId.ToString() },
                { "connectorId", ConnectorId },
                { "parameters", Parameters }
            }.RemoveNulls();
        }
    }
}
