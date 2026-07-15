using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// A credit card bill.
    /// </summary>
    public class Bill
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("accountId")]
        public Guid? AccountId { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("billClosingDate")]
        public DateTime? BillClosingDate { get; set; }

        [JsonProperty("totalAmount")]
        public double? TotalAmount { get; set; }

        [JsonProperty("totalAmountCurrencyCode")]
        public CurrencyCode TotalAmountCurrencyCode { get; set; }

        [JsonProperty("minimumPaymentAmount")]
        public double? MinimumPaymentAmount { get; set; }

        [JsonProperty("allowsInstallments")]
        public bool? AllowsInstallments { get; set; }

        [JsonProperty("financeCharges")]
        public List<BillFinanceCharge> FinanceCharges { get; set; }

        [JsonProperty("payments")]
        public List<BillPayment> Payments { get; set; }
    }

    public class BillFinanceCharge
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public BillFinanceChargeType? Type { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }

    public class BillPayment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("valueType")]
        public BillPaymentValueType? ValueType { get; set; }

        [JsonProperty("paymentDate")]
        public DateTime? PaymentDate { get; set; }

        [JsonProperty("paymentMode")]
        public BillPaymentMode? PaymentMode { get; set; }

        [JsonProperty("amount")]
        public double? Amount { get; set; }

        [JsonProperty("currencyCode")]
        public CurrencyCode CurrencyCode { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum BillFinanceChargeType
    {
        LATE_PAYMENT_REMUNERATIVE_INTEREST,
        LATE_PAYMENT_FEE,
        LATE_PAYMENT_INTEREST,
        IOF,
        OTHER
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum BillPaymentValueType
    {
        INSTALLMENT_PAYMENT,
        FULL_PAYMENT,
        OTHER_PAYMENT
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum BillPaymentMode
    {
        DEBIT_ACCOUNT,
        BANK_SLIP,
        PAYROLL_DEDUCTION,
        PIX
    }
}
