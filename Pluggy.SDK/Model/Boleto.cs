using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Pluggy.SDK.Helpers;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    // BETA: Boleto Management is a beta feature of the Pluggy API. The shape of these
    // models and endpoints may change. See https://docs.pluggy.ai for current status.

    #region Requests

    /// <summary>
    /// BETA. Request to create (issue) a boleto.
    /// </summary>
    public class CreateBoleto
    {
        public Guid BoletoConnectionId { get; set; }

        public CreateBoletoData Boleto { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "boletoConnectionId", BoletoConnectionId.ToString() },
                { "boleto", Boleto != null ? new Dictionary<string, object>
                    {
                        { "seuNumero", Boleto.SeuNumero },
                        { "amount", Boleto.Amount },
                        { "dueDate", Boleto.DueDate },
                        { "payer", Boleto.Payer != null ? new Dictionary<string, object>
                            {
                                { "taxNumber", Boleto.Payer.TaxNumber },
                                { "name", Boleto.Payer.Name },
                                { "addressStreet", Boleto.Payer.AddressStreet },
                                { "addressCity", Boleto.Payer.AddressCity },
                                { "addressState", Boleto.Payer.AddressState },
                                { "addressZipCode", Boleto.Payer.AddressZipCode }
                            }.RemoveNulls() : null
                        },
                        { "fine", Boleto.Fine != null ? new Dictionary<string, object>
                            {
                                { "value", Boleto.Fine.Value },
                                { "type", Boleto.Fine.Type?.ToString() }
                            }.RemoveNulls() : null
                        },
                        { "interest", Boleto.Interest != null ? new Dictionary<string, object>
                            {
                                { "value", Boleto.Interest.Value },
                                { "type", Boleto.Interest.Type?.ToString() }
                            }.RemoveNulls() : null
                        }
                    }.RemoveNulls() : null
                }
            }.RemoveNulls();
        }
    }

    public class CreateBoletoData
    {
        public string SeuNumero { get; set; }
        public double Amount { get; set; }
        public DateTime? DueDate { get; set; }
        public BoletoPayer Payer { get; set; }
        public BoletoFine Fine { get; set; }
        public BoletoInterest Interest { get; set; }
    }

    /// <summary>
    /// BETA. Request to create a boleto connection from credentials.
    /// </summary>
    public class CreateBoletoConnection
    {
        public long ConnectorId { get; set; }

        /// <summary>Credentials required for the connection (e.g. clientId, clientSecret, certificate, privateKey).</summary>
        public Dictionary<string, string> Credentials { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "connectorId", ConnectorId },
                { "credentials", Credentials }
            }.RemoveNulls();
        }
    }

    /// <summary>
    /// BETA. Request to create a boleto connection from an existing item.
    /// </summary>
    public class CreateBoletoConnectionFromItem
    {
        public Guid ItemId { get; set; }

        public Dictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>
            {
                { "itemId", ItemId.ToString() }
            }.RemoveNulls();
        }
    }

    #endregion

    #region Responses

    /// <summary>
    /// BETA. An issued boleto.
    /// </summary>
    public class IssuedBoleto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("status")]
        public IssuedBoletoStatus Status { get; set; }

        [JsonProperty("seuNumero")]
        public string SeuNumero { get; set; }

        [JsonProperty("dueDate")]
        public DateTime? DueDate { get; set; }

        [JsonProperty("payer")]
        public BoletoPayer Payer { get; set; }

        [JsonProperty("pixQr")]
        public string PixQr { get; set; }

        [JsonProperty("digitableLine")]
        public string DigitableLine { get; set; }

        [JsonProperty("nossoNumero")]
        public string NossoNumero { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("boletoConnectionId")]
        public Guid? BoletoConnectionId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("amountPaid")]
        public double? AmountPaid { get; set; }

        [JsonProperty("paymentOrigin")]
        public string PaymentOrigin { get; set; }

        [JsonProperty("fine")]
        public BoletoFine Fine { get; set; }

        [JsonProperty("interest")]
        public BoletoInterest Interest { get; set; }

        [JsonProperty("paidAt")]
        public DateTime? PaidAt { get; set; }
    }

    /// <summary>
    /// BETA. A boleto connection.
    /// </summary>
    public class BoletoConnection
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("connectorId")]
        public long ConnectorId { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }
    }

    #endregion

    #region Shared

    public class BoletoPayer
    {
        [JsonProperty("taxNumber")]
        public string TaxNumber { get; set; }

        [JsonProperty("personType")]
        public string PersonType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("addressStreet")]
        public string AddressStreet { get; set; }

        [JsonProperty("addressNumber")]
        public string AddressNumber { get; set; }

        [JsonProperty("addressComplement")]
        public string AddressComplement { get; set; }

        [JsonProperty("addressNeighborhood")]
        public string AddressNeighborhood { get; set; }

        [JsonProperty("addressCity")]
        public string AddressCity { get; set; }

        [JsonProperty("addressState")]
        public string AddressState { get; set; }

        [JsonProperty("addressZipCode")]
        public string AddressZipCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("ddd")]
        public string Ddd { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class BoletoFine
    {
        [JsonProperty("value")]
        public double? Value { get; set; }

        [JsonProperty("type")]
        public BoletoFineType? Type { get; set; }
    }

    public class BoletoInterest
    {
        [JsonProperty("value")]
        public double? Value { get; set; }

        [JsonProperty("type")]
        public BoletoInterestType? Type { get; set; }
    }

    #endregion

    #region Enums

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum IssuedBoletoStatus
    {
        OPEN,
        PAID,
        OVERDUE,
        CANCELLED,
        PROTESTED
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum BoletoFineType
    {
        PERCENTAGE,
        FIXED
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum BoletoInterestType
    {
        PERCENTAGE
    }

    #endregion
}
