using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum InvestmentStatus
    {
        ACTIVE,
        PENDING,
        TOTAL_WITHDRAWAL
    }
}
