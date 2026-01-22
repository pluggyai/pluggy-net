using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    public class CreatePaymentRecipientRequest
    {
        /// <summary>
        /// Recipient's full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recipient's tax number (CPF or CNPJ)
        /// </summary>
        public string TaxNumber { get; set; }

        /// <summary>
        /// Payment institution ID
        /// </summary>
        public Guid PaymentInstitutionId { get; set; }

        /// <summary>
        /// Bank account details
        /// </summary>
        public CreatePaymentRecipientAccountRequest Account { get; set; }

        /// <summary>
        /// PIX key (optional, alternative to account)
        /// </summary>
        public string PixKey { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "name", Name },
                { "taxNumber", TaxNumber },
                { "paymentInstitutionId", PaymentInstitutionId.ToString() },
                { "account", Account != null ? new Dictionary<string, object>
                    {
                        { "branch", Account.Branch },
                        { "number", Account.Number },
                        { "type", Account.Type?.ToString() }
                    }.RemoveNulls() : null
                },
                { "pixKey", PixKey }
            }.RemoveNulls();
        }
    }

    public class CreatePaymentRecipientAccountRequest
    {
        public string Branch { get; set; }
        public string Number { get; set; }
        public PaymentAccountType? Type { get; set; }
    }
}
