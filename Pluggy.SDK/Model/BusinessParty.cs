using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Partner or administrator of a business. Partners with less than 25% shareholding
    /// may be omitted by the institution.
    /// </summary>
    public class BusinessParty
    {
        [JsonProperty("type")]
        public BusinessPartyType Type { get; set; }

        [JsonProperty("personType")]
        public BusinessPartyPersonType PersonType { get; set; }

        [JsonProperty("documentType")]
        public BusinessPartyDocumentType DocumentType { get; set; }

        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>Issuing country of the document, alpha3 ISO-3166.</summary>
        [JsonProperty("documentCountry")]
        public string DocumentCountry { get; set; }

        [JsonProperty("documentExpirationDate")]
        public DateTime? DocumentExpirationDate { get; set; }

        [JsonProperty("documentIssueDate")]
        public DateTime? DocumentIssueDate { get; set; }

        [JsonProperty("documentAdditionalInfo")]
        public string DocumentAdditionalInfo { get; set; }

        /// <summary>Civil name. Required when PersonType is NATURAL_PERSON.</summary>
        [JsonProperty("civilName")]
        public string CivilName { get; set; }

        [JsonProperty("socialName")]
        public string SocialName { get; set; }

        /// <summary>Company name. Required when PersonType is LEGAL_ENTITY.</summary>
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("tradeName")]
        public string TradeName { get; set; }

        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Shareholding fraction between 0 and 1 (e.g. 0.51 represents 51%, 1 represents 100%).
        /// Required when Type is PARTNER and the shareholding is 25% or higher.
        /// </summary>
        [JsonProperty("shareholding")]
        public double? Shareholding { get; set; }
    }

    public enum BusinessPartyType
    {
        PARTNER,
        ADMINISTRATOR
    }

    public enum BusinessPartyPersonType
    {
        NATURAL_PERSON,
        LEGAL_ENTITY
    }

    public enum BusinessPartyDocumentType
    {
        CPF,
        CNPJ,
        PASSPORT,
        OTHER_TRAVEL_DOCUMENT
    }
}
