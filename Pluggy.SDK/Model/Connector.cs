using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Connector
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("institutionUrl")]
        public string InstitutionUrl { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("primaryColor")]
        public string PrimaryColor { get; set; }

        [JsonProperty("type")]
        public ConnectorType Type { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("credentials")]
        public IList<ConnectorParameter> Credentials { get; set; }

        [JsonProperty("oauth")]
        public bool? Oauth { get; set; }

        [JsonProperty("oauthUrl")]
        public string OauthUrl { get; set; }

        [JsonProperty("resetPasswordUrl")]
        public string ResetPasswordUrl { get; set; }

        [JsonProperty("isSandbox")]
        public bool IsSandbox { get; set; }

        [JsonProperty("isOpenFinance")]
        public bool IsOpenFinance { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("supportsPaymentInitiation")]
        public bool SupportsPaymentInitiation { get; set; }

        [JsonProperty("supportsScheduledPayments")]
        public bool SupportsScheduledPayments { get; set; }

        [JsonProperty("supportsSmartTransfers")]
        public bool SupportsSmartTransfers { get; set; }

        [JsonProperty("supportsAutomaticPix")]
        public bool SupportsAutomaticPix { get; set; }

        [JsonProperty("supportsBoletoManagement")]
        public bool SupportsBoletoManagement { get; set; }

        [JsonProperty("hasMFA")]
        public bool HasMFA { get; set; }

        [JsonProperty("health")]
        public ConnectorHealth Health { get; set; }

        [JsonProperty("products")]
        public IList<ProductType> Products { get; set; }
    }

    public class ConnectorHealth
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("stage")]
        public string Stage { get; set; }

        [JsonProperty("details")]
        public ConnectorHealthDetails Details { get; set; }
    }

    public class ConnectorHealthDetails
    {
        [JsonProperty("connectionRateLast6Hours")]
        public double? ConnectionRateLast6Hours { get; set; }

        [JsonProperty("connectionsLast6Hours")]
        public double? ConnectionsLast6Hours { get; set; }
    }
}
