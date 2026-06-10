using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Identity
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("birthDate")]
        public DateTime? BirthDate { get; set; }

        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("phoneNumbers")]
        public List<PhoneNumber> PhoneNumbers { get; set; }

        [JsonProperty("relations")]
        public List<IdentityRelation> Relations { get; set; }

        [JsonProperty("emails")]
        public List<Email> Emails { get; set; }

        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }

        [JsonProperty("investorProfile")]
        public InvestorProfile? InvestorProfile { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("establishmentCode")]
        public string EstablishmentCode { get; set; }

        [JsonProperty("establishmentName")]
        public string EstablishmentName { get; set; }

        /// <summary>Social name of the natural person (PF-only).</summary>
        [JsonProperty("socialName")]
        public string SocialName { get; set; }

        /// <summary>Sex of the natural person (PF-only).</summary>
        [JsonProperty("sex")]
        public Sex? Sex { get; set; }

        /// <summary>Marital status of the natural person (PF-only).</summary>
        [JsonProperty("maritalStatus")]
        public MaritalStatus MaritalStatus { get; set; }

        /// <summary>Nationality of the natural person (PF-only).</summary>
        [JsonProperty("nationality")]
        public Nationality Nationality { get; set; }

        /// <summary>Other identification documents the natural person holds (PF-only).</summary>
        [JsonProperty("otherDocuments")]
        public List<OtherDocument> OtherDocuments { get; set; }

        /// <summary>Passport metadata for the natural person (PF-only).</summary>
        [JsonProperty("passport")]
        public Passport Passport { get; set; }

        /// <summary>Date the business was incorporated (PJ-only).</summary>
        [JsonProperty("incorporationDate")]
        public DateTime? IncorporationDate { get; set; }

        /// <summary>Partners and administrators of the business (PJ-only).</summary>
        [JsonProperty("parties")]
        public List<BusinessParty> Parties { get; set; }

        /// <summary>
        /// Additional documents for businesses headquartered abroad and not required to register a CNPJ (PJ-only).
        /// </summary>
        [JsonProperty("businessOtherDocuments")]
        public List<BusinessOtherDocument> BusinessOtherDocuments { get; set; }

        /// <summary>CNPJs of the financial institutions responsible for the customer cadastro.</summary>
        [JsonProperty("companiesCnpj")]
        public List<string> CompaniesCnpj { get; set; }

        /// <summary>
        /// Information that allows institutions to assess and classify the client's risk profile
        /// and economic-financial capacity (Open Finance).
        /// </summary>
        [JsonProperty("financialRelationships")]
        public IdentityFinancialRelationships FinancialRelationships { get; set; }

        /// <summary>
        /// Information about since when the person has been a client of the institution and the
        /// products/services they consume, plus income/patrimony data (Open Finance).
        /// </summary>
        [JsonProperty("qualifications")]
        public IdentityQualifications Qualifications { get; set; }
    }
}
