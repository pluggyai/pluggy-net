using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    public class Loan
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("contractNumber")]
        public string ContractNumber { get; set; }

        [JsonProperty("ipocCode")]
        public string IpocCode { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("contractDate")]
        public DateTime? ContractDate { get; set; }

        [JsonProperty("disbursementDates")]
        public List<string> DisbursementDates { get; set; }

        [JsonProperty("settlementDate")]
        public DateTime? SettlementDate { get; set; }

        [JsonProperty("contractAmount")]
        public double? ContractAmount { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("instalmentPeriodicity")]
        public LoanInstalmentPeriodicity? InstalmentPeriodicity { get; set; }

        [JsonProperty("instalmentPeriodicityAdditionalInfo")]
        public string InstalmentPeriodicityAdditionalInfo { get; set; }

        [JsonProperty("firstInstalmentDueDate")]
        public DateTime? FirstInstalmentDueDate { get; set; }

        [JsonProperty("CET")]
        public double? CET { get; set; }

        [JsonProperty("amortizationScheduled")]
        public LoanAmortizationScheduled? AmortizationScheduled { get; set; }

        [JsonProperty("amortizationScheduledAdditionalInfo")]
        public string AmortizationScheduledAdditionalInfo { get; set; }

        [JsonProperty("cnpjConsignee")]
        public string CnpjConsignee { get; set; }

        [JsonProperty("interestRates")]
        public List<LoanInterestRate> InterestRates { get; set; }

        [JsonProperty("contractedFees")]
        public List<LoanContractedFee> ContractedFees { get; set; }

        [JsonProperty("contractedFinanceCharges")]
        public List<LoanFinanceCharge> ContractedFinanceCharges { get; set; }

        [JsonProperty("warranties")]
        public List<LoanWarranty> Warranties { get; set; }

        [JsonProperty("installments")]
        public LoanInstallments Installments { get; set; }

        [JsonProperty("payments")]
        public LoanPayments Payments { get; set; }
    }

    public class LoanInterestRate
    {
        [JsonProperty("taxType")]
        public LoanTaxType? TaxType { get; set; }

        [JsonProperty("interestRateType")]
        public LoanInterestRateType? InterestRateType { get; set; }

        [JsonProperty("taxPeriodicity")]
        public LoanTaxPeriodicity? TaxPeriodicity { get; set; }

        [JsonProperty("calculation")]
        public string Calculation { get; set; }

        [JsonProperty("referentialRateIndexerType")]
        public string ReferentialRateIndexerType { get; set; }

        [JsonProperty("referentialRateIndexerSubType")]
        public string ReferentialRateIndexerSubType { get; set; }

        [JsonProperty("referentialRateIndexerAdditionalInfo")]
        public string ReferentialRateIndexerAdditionalInfo { get; set; }

        [JsonProperty("preFixedRate")]
        public double? PreFixedRate { get; set; }

        [JsonProperty("postFixedRate")]
        public double? PostFixedRate { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    public class LoanContractedFee
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("chargeType")]
        public LoanChargeType? ChargeType { get; set; }

        [JsonProperty("charge")]
        public LoanCharge? Charge { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("rate")]
        public double? Rate { get; set; }
    }

    public class LoanFinanceCharge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }

        [JsonProperty("rate")]
        public double? Rate { get; set; }
    }

    public class LoanWarranty
    {
        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subtype")]
        public string Subtype { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }
    }

    public class LoanInstallments
    {
        [JsonProperty("typeNumberOfInstallments")]
        public LoanPeriodType? TypeNumberOfInstallments { get; set; }

        [JsonProperty("totalNumberOfInstallments")]
        public int? TotalNumberOfInstallments { get; set; }

        [JsonProperty("typeContractRemaining")]
        public LoanContractRemainingType? TypeContractRemaining { get; set; }

        [JsonProperty("contractRemainingNumber")]
        public int? ContractRemainingNumber { get; set; }

        [JsonProperty("paidInstallments")]
        public int? PaidInstallments { get; set; }

        [JsonProperty("dueInstallments")]
        public int? DueInstallments { get; set; }

        [JsonProperty("pastDueInstallments")]
        public int? PastDueInstallments { get; set; }

        [JsonProperty("balloonPayments")]
        public List<LoanBalloonPayment> BalloonPayments { get; set; }
    }

    public class LoanBalloonPayment
    {
        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("amount")]
        public LoanBalloonPaymentAmount Amount { get; set; }
    }

    public class LoanBalloonPaymentAmount
    {
        [JsonProperty("value")]
        public double? Value { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }
    }

    public class LoanPayments
    {
        [JsonProperty("contractOutstandingBalance")]
        public double? ContractOutstandingBalance { get; set; }

        [JsonProperty("releases")]
        public List<LoanPaymentRelease> Releases { get; set; }
    }

    public class LoanPaymentRelease
    {
        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        [JsonProperty("isOverParcelPayment")]
        public bool? IsOverParcelPayment { get; set; }

        [JsonProperty("instalmentId")]
        public string InstalmentId { get; set; }

        [JsonProperty("paidDate")]
        public DateTime? PaidDate { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("paidAmount")]
        public double? PaidAmount { get; set; }

        [JsonProperty("overParcel")]
        public LoanOverParcel OverParcel { get; set; }
    }

    public class LoanOverParcel
    {
        [JsonProperty("fees")]
        public List<LoanOverParcelFee> Fees { get; set; }

        [JsonProperty("charges")]
        public List<LoanOverParcelCharge> Charges { get; set; }
    }

    public class LoanOverParcelFee
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }
    }

    public class LoanOverParcelCharge
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }
    }

    #region Loan Enums

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanInstalmentPeriodicity
    {
        WITHOUT_REGULAR_PERIODICITY,
        WEEKLY,
        FORTNIGHTLY,
        MONTHLY,
        BIMONTHLY,
        QUARTERLY,
        SEMESTERLY,
        YEARLY,
        OTHERS
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanAmortizationScheduled
    {
        SAC,
        PRICE,
        SAM,
        WITHOUT_AMORTIZATION_SYSTEM,
        OTHERS
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanTaxType
    {
        NOMINAL,
        EFFECTIVE
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanInterestRateType
    {
        SIMPLE,
        COMPOUND
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanTaxPeriodicity
    {
        MONTHLY,
        YEARLY
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanChargeType
    {
        UNIQUE,
        BY_INSTALLMENT
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanCharge
    {
        MINIMUM,
        MAXIMUM,
        FIXED,
        PERCENTAGE
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanPeriodType
    {
        DAY,
        WEEK,
        MONTH,
        YEAR,
        WITHOUT_TOTAL_PERIOD
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum LoanContractRemainingType
    {
        DAY,
        WEEK,
        MONTH,
        YEAR,
        WITHOUT_TOTAL_PERIOD,
        WITHOUT_REMAINING_PERIOD
    }

    #endregion
}
