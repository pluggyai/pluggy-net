using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Point-in-time balance of an account (GET /accounts/{id}/balance).
    /// </summary>
    public class AccountBalance
    {
        /// <summary>The available balance of the account.</summary>
        [JsonProperty("balance")]
        public double Balance { get; set; }

        /// <summary>The currency code of the balance amounts.</summary>
        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        /// <summary>The date and time when the balance was last updated.</summary>
        [JsonProperty("updateDateTime")]
        public DateTime? UpdateDateTime { get; set; }
    }
}
