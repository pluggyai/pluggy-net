using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum CreditCardAccountOtherCreditType
    {
        REVOLVING_CREDIT,
        BILL_INSTALLMENT,
        LOAN,
        OTHER,
    }
}
