using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    public class CreatePaymentCustomerRequest
    {
        /// <summary>
        /// Customer type: INDIVIDUAL or BUSINESS
        /// </summary>
        public PaymentCustomerType Type { get; set; }

        /// <summary>
        /// Customer's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer's email (optional)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Customer's CPF (required for both INDIVIDUAL and BUSINESS)
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Customer's CNPJ (required for BUSINESS type)
        /// </summary>
        public string Cnpj { get; set; }

        /// <summary>
        /// Connector ID to pre-select institution (optional)
        /// </summary>
        public long? ConnectorId { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "type", Type.ToString() },
                { "name", Name },
                { "email", Email },
                { "cpf", Cpf },
                { "cnpj", Cnpj },
                { "connectorId", ConnectorId }
            }.RemoveNulls();
        }
    }
}
