using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum AccountSubtype { SAVINGS_ACCOUNT, CHECKING_ACCOUNT, CREDIT_CARD }
}


