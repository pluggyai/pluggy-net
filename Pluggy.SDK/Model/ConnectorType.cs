using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum ConnectorType
    {
        PERSONAL_BANK,
        BUSINESS_BANK,
        INVESTMENT
    }
}
