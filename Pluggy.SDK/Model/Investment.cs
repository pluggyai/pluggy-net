using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Investment
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("isin")]
        public string? Isin { get; set; }

        [JsonProperty("type")]
        public InvestmentType Type { get; set; }

        [JsonProperty("subtype")]
        public InvestmentSubtype? Subtype { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("balance")]
        public double? Balance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("lastMonthRate")]
        public double? LastMonthRate { get; set; }

        [JsonProperty("lastTwelveMonthsRate")]
        public double? LastTwelveMonthsRate { get; set; }

        [JsonProperty("annualRate")]
        public double? AnnualRate { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("value")]
        public double? Value { get; set; }

        [JsonProperty("quantity")]
        public double? Quantity { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("taxes")]
        public double? Taxes { get; set; }

        [JsonProperty("taxes2")]
        public double? Taxes2 { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("amountWithdrawal")]
        public double? AmountWithdrawal { get; set; }

        [JsonProperty("amountProfit")]
        public double? AmountProfit { get; set; }

        [JsonProperty("amountOriginal")]
        public double? AmountOriginal { get; set; }

        [JsonProperty("issueDate")]
        public DateTime IssueDate { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("rateType")]
        public string RateType { get; set; }

        [JsonProperty("fixedAnnualRate")]
        public double? FixedAnnualRate { get; set; }

        [JsonProperty("status")]
        public InvestmentStatus Status { get; set; }

        [JsonProperty("institution")]
        public InvestmentInstitution Institution { get; set; }

        [JsonProperty("transactions")]
        public List<InvestmentTransaction> Transactions { get; set; }

        [JsonProperty("metadata")]
        public InvestmentMetadata? Metadata { get; set; }

        [JsonProperty("providerId")]
        public string? ProviderId { get; set; }
    }
}
