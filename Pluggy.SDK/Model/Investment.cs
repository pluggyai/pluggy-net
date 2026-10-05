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
        public string ISIN { get; set; }

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
        public DateTime Date { get; set; }

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
        public DateTime? IssueDate { get; set; }

        [JsonProperty("purchaseDate")]
        public DateTime? PurchaseDate { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("issuerCNPJ")]
        public string IssuerCNPJ { get; set; }

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

        [Obsolete("Use method client.FetchInvestmentTransactions(investmentId, TransactionParameters) instead, this field is null unless the application was created before 2023-03-21", false)]
        [JsonProperty("transactions")]
        public List<InvestmentTransaction> Transactions { get; set; }

        [JsonProperty("metadata")]
        public InvestmentMetadata Metadata { get; set; }

        /// <summary>Coupon-payment schedule for coupon-bearing fixed income and Treasury bonds.</summary>
        [JsonProperty("couponPayment")]
        public InvestmentCouponPayment CouponPayment { get; set; }

        /// <summary>Underlying debtor of receivables-backed paper (CRI / CRA).</summary>
        [JsonProperty("debtor")]
        public InvestmentDebtor Debtor { get; set; }

        /// <summary>Date when the grace period ends (fixed-income investments only).</summary>
        [JsonProperty("gracePeriodDate")]
        public DateTime? GracePeriodDate { get; set; }

        /// <summary>B3 lot/price conversion factor (variable income).</summary>
        [JsonProperty("priceFactor")]
        public double? PriceFactor { get; set; }

        /// <summary>Whether the product is tax-exempt (LCI, LCA, CRI, CRA, incentivized debentures).</summary>
        [JsonProperty("taxExempt")]
        public bool? TaxExempt { get; set; }
    }
}
