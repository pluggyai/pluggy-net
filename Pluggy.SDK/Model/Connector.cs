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

        /// <summary>
        /// Incidents affecting this connector right now, worst first. Null when
        /// there are none, so its presence is the signal.
        ///
        /// It describes the institution, not your own connections — that is
        /// <see cref="Details"/>. Status answers "can I connect at all";
        /// this answers "what is wrong". A bank can be perfectly reachable and
        /// still be failing to return instalments.
        /// </summary>
        [JsonProperty("incidents")]
        public IList<ConnectorIncident> Incidents { get; set; }

        [JsonProperty("details")]
        public ConnectorHealthDetails Details { get; set; }
    }

    /// <summary>
    /// An incident affecting a connector right now, as published on
    /// https://status.pluggy.ai. Only incidents active at this moment are
    /// listed: a scheduled maintenance appears once its window opens, not when
    /// it is announced.
    /// </summary>
    public class ConnectorIncident
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>Short, customer-facing summary. Safe to show to your own users.</summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public ConnectorIncidentType Type { get; set; }

        /// <summary>
        /// Affected product line: dados, pis, pis-agendado, pixauto, smart or
        /// infra. A string rather than an enum because those values are not
        /// valid identifiers.
        /// </summary>
        [JsonProperty("product")]
        public string Product { get; set; }

        /// <summary>INCIDENT for an unplanned problem, MAINTENANCE for a planned window.</summary>
        [JsonProperty("kind")]
        public string Kind { get; set; }

        /// <summary>DEGRADED, PARTIAL_OUTAGE, MAJOR_OUTAGE or MAINTENANCE.</summary>
        [JsonProperty("severity")]
        public string Severity { get; set; }

        /// <summary>INVESTIGATING, IDENTIFIED, MONITORING or SCHEDULED. Resolved incidents are not listed.</summary>
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("startedAt")]
        public DateTime StartedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Permalink to the full timeline and postmortem on the status page.</summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class ConnectorHealthDetails
    {
        [JsonProperty("connectionRateLast6Hours")]
        public double? ConnectionRateLast6Hours { get; set; }

        [JsonProperty("connectionsLast6Hours")]
        public double? ConnectionsLast6Hours { get; set; }
    }
}
