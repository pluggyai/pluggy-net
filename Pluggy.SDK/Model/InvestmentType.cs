using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum InvestmentType
    {
        MUTUAL_FUND,
        SECURITY,
        EQUITY,
        FIXED_INCOME,
        ETF,
        COE,
        OTHER,
    }
}
