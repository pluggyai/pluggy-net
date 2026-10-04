using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Whether a credit card purchase is charged in full on a single bill or split into installments.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum CreditCardAccountPaymentType
    {
        SINGLE,
        INSTALLMENT,
    }
}
