using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionCreditCardMetadata
    {
        [JsonProperty("installmentNumber")]
        public int? InstallmentNumber { get; set; }

        [JsonProperty("totalInstallments")]
        public int? TotalInstallments { get; set; }

        [JsonProperty("totalAmount")]
        public double? TotalAmount { get; set; }
        
        [JsonProperty("payeeMCC")]
        public string PayeeMCC { get; set; }

        [JsonProperty("purchaseDate")]
        public DateTime? PurchaseDate { get; set; }

        [JsonProperty("billId")]
        public string BillId { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        // Type of fee charged. Present when the operation is a fee (TARIFA).
        [JsonProperty("feeType")]
        public CreditCardAccountFeeType? FeeType { get; set; }

        // Free text describing the fee type when feeType is "OTHER".
        [JsonProperty("feeTypeAdditionalInfo")]
        public string FeeTypeAdditionalInfo { get; set; }

        // Other type of credit contracted on the card. Present when the operation is a contracted credit operation.
        [JsonProperty("otherCreditsType")]
        public CreditCardAccountOtherCreditType? OtherCreditsType { get; set; }

        // Free text describing the other credit type when otherCreditsType is "OTHER".
        [JsonProperty("otherCreditsAdditionalInfo")]
        public string OtherCreditsAdditionalInfo { get; set; }

        // Forecasted bill period (formatted as YYYY-MM) in which this transaction is expected to be charged.
        // Unlike BillId, it is provided for pending and future transactions too. Only returned for Open Finance connectors.
        [JsonProperty("billForecastDate")]
        public string BillForecastDate { get; set; }

        // Whether the purchase is charged in full on a single bill (SINGLE) or split into installments (INSTALLMENT).
        // Only returned for Open Finance connectors.
        [JsonProperty("paymentType")]
        public CreditCardAccountPaymentType? PaymentType { get; set; }

        // Date (YYYY-MM-DD) the institution posted the transaction to a bill, as reported by the institution and never
        // adjusted by Pluggy. Null when the transaction is not posted to a bill yet or the institution does not report it.
        // Only returned for Open Finance connectors.
        [JsonProperty("billPostDate")]
        public string BillPostDate { get; set; }

        // Date and time of the transaction as reported by the institution, normalized to ISO-8601.
        // Kept as the raw string the institution sent. Only returned for Open Finance connectors.
        [JsonProperty("transactionDateTime")]
        public string TransactionDateTime { get; set; }
    }
}

