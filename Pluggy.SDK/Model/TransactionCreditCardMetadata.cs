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
    }
}

