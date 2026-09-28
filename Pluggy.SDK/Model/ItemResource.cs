using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// One resource the financial institution declared for an item's Open Finance
    /// consent, reported verbatim.
    /// </summary>
    public class ItemResource
    {
        /// <summary>The institution's identifier for the resource.</summary>
        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        /// <summary>
        /// Open Finance resource type. Known values are in <see cref="ItemResourceType"/>;
        /// a string rather than an enum because institutions can report others.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// What the institution reports about this resource. Known values are in
        /// <see cref="ItemResourceStatus"/>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public static class ItemResourceType
    {
        public const string ACCOUNT = "ACCOUNT";
        public const string CREDIT_CARD_ACCOUNT = "CREDIT_CARD_ACCOUNT";
        public const string LOAN = "LOAN";
        public const string FINANCING = "FINANCING";
        public const string UNARRANGED_ACCOUNT_OVERDRAFT = "UNARRANGED_ACCOUNT_OVERDRAFT";
        public const string INVOICE_FINANCING = "INVOICE_FINANCING";
        public const string BANK_FIXED_INCOME = "BANK_FIXED_INCOME";
        public const string CREDIT_FIXED_INCOME = "CREDIT_FIXED_INCOME";
        public const string VARIABLE_INCOME = "VARIABLE_INCOME";
        public const string TREASURE_TITLE = "TREASURE_TITLE";
        public const string FUND = "FUND";
    }

    /// <summary>
    /// Note the British spelling of PENDING_AUTHORISATION: it is Open Finance's, kept verbatim.
    /// </summary>
    public static class ItemResourceStatus
    {
        public const string AVAILABLE = "AVAILABLE";
        public const string UNAVAILABLE = "UNAVAILABLE";
        public const string TEMPORARILY_UNAVAILABLE = "TEMPORARILY_UNAVAILABLE";
        public const string PENDING_AUTHORISATION = "PENDING_AUTHORISATION";
    }
}
