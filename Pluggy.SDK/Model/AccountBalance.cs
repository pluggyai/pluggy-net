using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Point-in-time balance of an account (GET /accounts/{id}/balance).
    /// </summary>
    public class AccountBalance
    {
        /// <summary>The available balance of the account, same as the account resource's balance.</summary>
        [JsonProperty("balance")]
        public double Balance { get; set; }

        /// <summary>Amount currently blocked or held on the account. Null when the institution does not report it.</summary>
        [JsonProperty("blockedBalance")]
        public double? BlockedBalance { get; set; }

        /// <summary>Amount held in the account's automatic investment facility. Null when the institution does not report it.</summary>
        [JsonProperty("automaticallyInvestedBalance")]
        public double? AutomaticallyInvestedBalance { get; set; }

        /// <summary>
        /// Accounting balance of the account: balance plus blocked balance. Same value as the account
        /// resource's bankData.closingBalance, which the API refreshes with it. Null when the institution does not report it.
        /// </summary>
        [JsonProperty("closingBalance")]
        public double? ClosingBalance { get; set; }

        /// <summary>The currency code of the balance amounts.</summary>
        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        /// <summary>The date and time when the balance was last updated.</summary>
        [JsonProperty("updateDateTime")]
        public DateTime? UpdateDateTime { get; set; }
    }
}
