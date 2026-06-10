using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Qualification information about the client: occupation, income, patrimony and, for businesses,
    /// economic activities. Exposed by Open Finance connectors.
    /// </summary>
    public class IdentityQualifications
    {
        [JsonProperty("companyCnpj")]
        public string CompanyCnpj { get; set; }

        [JsonProperty("occupationCode")]
        public IdentityOccupationCode? OccupationCode { get; set; }

        /// <summary>
        /// Free-text occupation description. Holds the standardized list code when OccupationCode is
        /// RECEITA_FEDERAL or CBO; describes the occupation when OUTRO.
        /// </summary>
        [JsonProperty("occupationDescription")]
        public string OccupationDescription { get; set; }

        [JsonProperty("informedIncome")]
        public IdentityInformedIncome InformedIncome { get; set; }

        [JsonProperty("informedPatrimony")]
        public IdentityInformedPatrimony InformedPatrimony { get; set; }

        /// <summary>CNAE codes describing the economic activities of the business. Only one may be marked as main.</summary>
        [JsonProperty("economicActivities")]
        public List<EconomicActivity> EconomicActivities { get; set; }

        [JsonProperty("informedRevenue")]
        public InformedRevenue InformedRevenue { get; set; }
    }

    public class IdentityInformedIncome
    {
        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }

    public class IdentityInformedPatrimony
    {
        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("year")]
        public double? Year { get; set; }

        /// <summary>Reference date of the patrimony (returned on the business path of Open Finance).</summary>
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }

    public class EconomicActivity
    {
        /// <summary>CNAE code (7 digits, including leading zeros). Follows the CNAE-Subclasse 2.3 classification.</summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>Whether this is the main economic activity (true) or a secondary one (false).</summary>
        [JsonProperty("isMain")]
        public bool? IsMain { get; set; }
    }

    public class InformedRevenue
    {
        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("frequency")]
        public InformedRevenueFrequency? Frequency { get; set; }

        /// <summary>Free-text complement to the frequency (when Frequency is OTHER).</summary>
        [JsonProperty("frequencyAdditionalInfo")]
        public string FrequencyAdditionalInfo { get; set; }

        [JsonProperty("year")]
        public double? Year { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum IdentityOccupationCode
    {
        RECEITA_FEDERAL,
        CBO,
        OUTRO
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum InformedRevenueFrequency
    {
        DAILY,
        WEEKLY,
        BIWEEKLY,
        MONTHLY,
        BIMONTHLY,
        QUARTERLY,
        SEMIANNUAL,
        ANNUAL,
        OTHER
    }
}
