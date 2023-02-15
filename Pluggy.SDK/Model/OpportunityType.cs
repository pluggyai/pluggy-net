using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum OpportunityType
    {
        CREDIT_CARD,
        PERSONAL_LOAN,
        BUSINESS_LOAN,
        MORTGAGE_LOAN,
        VEHICLE_LOAN,
        OVERDRAFT,
        OTHER_LOAN,
        OTHER,
    }
}
