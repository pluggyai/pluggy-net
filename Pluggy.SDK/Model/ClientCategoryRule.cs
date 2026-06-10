using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// A client-defined transaction categorization rule.
    /// </summary>
    public class ClientCategoryRule
    {
        /// <summary>Description of the transaction rule.</summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>Identifier of the category.</summary>
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        /// <summary>Description of the category.</summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>Identifier of the client.</summary>
        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        /// <summary>Transaction type (DEBIT/CREDIT).</summary>
        [JsonProperty("transactionType")]
        public string TransactionType { get; set; }

        /// <summary>Account type (CHECKING_ACCOUNT/CREDIT_CARD).</summary>
        [JsonProperty("accountType")]
        public string AccountType { get; set; }
    }

    /// <summary>
    /// Request to create a client category rule.
    /// </summary>
    public class CreateClientCategoryRule
    {
        /// <summary>Description of the transaction rule.</summary>
        public string Description { get; set; }

        /// <summary>Identifier of the category.</summary>
        public string CategoryId { get; set; }

        /// <summary>Transaction type (DEBIT/CREDIT) (optional).</summary>
        public string TransactionType { get; set; }

        /// <summary>Account type (CHECKING_ACCOUNT/CREDIT_CARD) (optional).</summary>
        public string AccountType { get; set; }

        /// <summary>Match type (exact/contains/startsWith/endsWith). Defaults to 'exact'.</summary>
        public string MatchType { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "description", Description },
                { "categoryId", CategoryId },
                { "transactionType", TransactionType },
                { "accountType", AccountType },
                { "matchType", MatchType }
            }.RemoveNulls();
        }
    }
}
