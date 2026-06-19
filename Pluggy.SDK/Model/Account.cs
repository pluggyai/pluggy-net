using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{

    public class Account
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("marketingName")]
        public string MarketingName { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("balance")]
        public double Balance { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("type")]
        public AccountType Type { get; set; }

        [JsonProperty("subtype")]
        public AccountSubtype Subtype { get; set; }

        [JsonProperty("creditData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public CreditAccount CreditData { get; set; }

        [JsonProperty("bankData", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public BankAccount BankData { get; set; }

        [JsonProperty("transactions")]
        public ICollection<Transaction> Transactions { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

    }

    public class BankAccount
    {
        [JsonProperty("transferNumber")]
        public string TransferNumber { get; set; }

        [JsonProperty("closingBalance")]
        public double? ClosingBalance { get; set; }

        [JsonProperty("automaticallyInvestedBalance")]
        public double? AutomaticallyInvestedBalance { get; set; }

        [JsonProperty("overdraftContractedLimit")]
        public double? OverdraftContractedLimit { get; set; }

        [JsonProperty("unarrangedOverdraftAmount")]
        public double? UnarrangedOverdraftAmount { get; set; }

        [JsonProperty("hasReservedBalance")]
        public bool? HasReservedBalance { get; set; }

        [JsonProperty("reservedBalances")]
        public ICollection<ReservedBalance> ReservedBalances { get; set; }
    }

    public class CreditAccount
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        // Free text to specify the brand category when brand is marked as "OTHER".
        [JsonProperty("brandAdditionalInfo")]
        public string BrandAdditionalInfo { get; set; }

        [JsonProperty("balanceCloseDate")]
        public DateTime? BalanceCloseDate { get; set; }

        [JsonProperty("balanceDueDate")]
        public DateTime? BalanceDueDate { get; set; }

        [JsonProperty("availableCreditLimit")]
        public double? AvailableCreditLimit { get; set; }

        [JsonProperty("creditLimit")]
        public double? CreditLimit { get; set; }

        [JsonProperty("balanceForeignCurrency")]
        public double? BalanceForeignCurrency { get; set; }

        [JsonProperty("minimumPayment")]
        public double? MinimumPayment { get; set; }

        [JsonProperty("isLimitFlexible")]
        public bool? IsLimitFlexible { get; set; }

        [JsonProperty("holderType")]
        public string HolderType { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("disaggregatedCreditLimits")]
        public ICollection<DisaggregatedCreditLimit> DisaggregatedCreditLimits { get; set; }

    }

    public class DisaggregatedCreditLimit
    {
        [JsonProperty("creditLineLimitType")]
        public string CreditLineLimitType { get; set; }

        [JsonProperty("consolidationType")]
        public string ConsolidationType { get; set; }

        [JsonProperty("identificationNumber")]
        public string IdentificationNumber { get; set; }

        [JsonProperty("isLimitFlexible")]
        public bool? IsLimitFlexible { get; set; }

        [JsonProperty("lineName")]
        public CreditCardLimitLineName LineName { get; set; }

        [JsonProperty("lineNameAdditionalInfo")]
        public string LineNameAdditionalInfo { get; set; }

        [JsonProperty("limitAmount")]
        public double? LimitAmount { get; set; }

        [JsonProperty("limitAmountCurrencyCode")]
        public CurrencyCode? LimitAmountCurrencyCode { get; set; }

        // Reason why the reported total limit amount is equal to zero.
        [JsonProperty("limitAmountReason")]
        public string LimitAmountReason { get; set; }

        [JsonProperty("usedAmount")]
        public double? UsedAmount { get; set; }

        [JsonProperty("usedAmountCurrencyCode")]
        public CurrencyCode? UsedAmountCurrencyCode { get; set; }

        [JsonProperty("availableAmount")]
        public double? AvailableAmount { get; set; }

        [JsonProperty("availableAmountCurrencyCode")]
        public CurrencyCode? AvailableAmountCurrencyCode { get; set; }

        // Total limit amount customized by the customer through the institution's electronic channels.
        [JsonProperty("customizedLimitAmount")]
        public double? CustomizedLimitAmount { get; set; }

        // Currency of the reported customized limit amount.
        [JsonProperty("customizedLimitAmountCurrencyCode")]
        public CurrencyCode? CustomizedLimitAmountCurrencyCode { get; set; }
    }
}


