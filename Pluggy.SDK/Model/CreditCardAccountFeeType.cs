using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum CreditCardAccountFeeType
    {
        ANNUAL_FEE,
        ATM_WITHDRAWAL_DOMESTIC,
        ATM_WITHDRAWAL_INTERNATIONAL,
        EMERGENCY_CREDIT_EVALUATION,
        CARD_REISSUE,
        BILL_PAYMENT_FEE,
        SMS,
        OTHER,
    }
}
