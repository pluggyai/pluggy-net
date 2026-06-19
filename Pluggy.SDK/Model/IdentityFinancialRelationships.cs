using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Financial relationship information used to assess, characterize and classify the client.
    /// Exposed by Open Finance connectors.
    /// </summary>
    public class IdentityFinancialRelationships
    {
        /// <summary>Date when the relationship with the institution started.</summary>
        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>List of products and services that the client consumes.</summary>
        [JsonProperty("productsServicesType")]
        public List<string> ProductsServicesType { get; set; }

        /// <summary>Additional info about the products and services (when productsServicesType includes 'OUTROS').</summary>
        [JsonProperty("productsServicesTypeAdditionalInfo")]
        public string ProductsServicesTypeAdditionalInfo { get; set; }

        /// <summary>List of procurators of the client.</summary>
        [JsonProperty("procurators")]
        public List<IdentityProcurator> Procurators { get; set; }

        /// <summary>Accounts of the client with valid consent.</summary>
        [JsonProperty("accounts")]
        public List<FinancialRelationshipAccount> Accounts { get; set; }

        /// <summary>Salary portabilities received from the client's previous paycheck banks (PF-only).</summary>
        [JsonProperty("portabilitiesReceived")]
        public List<PortabilityReceived> PortabilitiesReceived { get; set; }

        /// <summary>Paycheck-bank (banco-folha) links to employers, active or formerly active (PF-only).</summary>
        [JsonProperty("paychecksBankLink")]
        public List<PaycheckBankLink> PaychecksBankLink { get; set; }
    }

    public class IdentityProcurator
    {
        [JsonProperty("type")]
        public IdentityProcuratorType? Type { get; set; }

        /// <summary>CPF of the procurator (may hold a CPF or CNPJ for business links; prefer DocumentNumber + DocumentType).</summary>
        [JsonProperty("cpfNumber")]
        public string CpfNumber { get; set; }

        /// <summary>Document number of the procurator (CPF or CNPJ). Canonical value to read.</summary>
        [JsonProperty("documentNumber")]
        public string DocumentNumber { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        [JsonProperty("civilName")]
        public string CivilName { get; set; }

        [JsonProperty("socialName")]
        public string SocialName { get; set; }
    }

    public class FinancialRelationshipAccount
    {
        [JsonProperty("compeCode")]
        public string CompeCode { get; set; }

        [JsonProperty("branchCode")]
        public string BranchCode { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("checkDigit")]
        public string CheckDigit { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subtype")]
        public string Subtype { get; set; }
    }

    public class PortabilityReceived
    {
        [JsonProperty("employerName")]
        public string EmployerName { get; set; }

        [JsonProperty("employerDocument")]
        public string EmployerDocument { get; set; }

        [JsonProperty("paycheckBankDetainerCnpj")]
        public string PaycheckBankDetainerCnpj { get; set; }

        [JsonProperty("paycheckBankDetainerIspb")]
        public string PaycheckBankDetainerIspb { get; set; }

        [JsonProperty("portabilityApprovalDate")]
        public DateTime? PortabilityApprovalDate { get; set; }
    }

    public class PaycheckBankLink
    {
        [JsonProperty("employerName")]
        public string EmployerName { get; set; }

        [JsonProperty("employerDocument")]
        public string EmployerDocument { get; set; }

        [JsonProperty("paycheckBankCnpj")]
        public string PaycheckBankCnpj { get; set; }

        [JsonProperty("paycheckBankIspb")]
        public string PaycheckBankIspb { get; set; }

        [JsonProperty("accountOpeningDate")]
        public DateTime? AccountOpeningDate { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum IdentityProcuratorType
    {
        REPRESENTANTE_LEGAL,
        PROCURADOR
    }
}
