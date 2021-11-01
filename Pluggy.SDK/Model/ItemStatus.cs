using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum ItemStatus
    {
        // Connection was succesffully completed
        UPDATED,

        // Connection encountered errors
        OUTDATED,

        // Credentials are invalid
        LOGIN_ERROR,

        // Connection is syncing
        UPDATING,

        // Connection is waiting for user's input
        WAITING_USER_INPUT,

        // Connection request was accepted
        CREATED
    }
}
