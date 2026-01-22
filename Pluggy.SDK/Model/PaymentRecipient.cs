using System;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    public class PaymentRecipient
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("isDefault")]
        public bool? IsDefault { get; set; }

        [JsonProperty("paymentInstitution")]
        public PaymentInstitution PaymentInstitution { get; set; }

        [JsonProperty("account")]
        public PaymentRecipientAccount Account { get; set; }

        [JsonProperty("pixKey")]
        public string PixKey { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentInstitution
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tradeName")]
        public string TradeName { get; set; }

        [JsonProperty("ispb")]
        public string Ispb { get; set; }

        [JsonProperty("compe")]
        public string Compe { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class PaymentRecipientAccount
    {
        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("type")]
        public PaymentAccountType? Type { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum PaymentAccountType
    {
        CHECKING_ACCOUNT,
        SAVINGS_ACCOUNT
    }
}
