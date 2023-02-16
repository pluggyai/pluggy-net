using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ExecutionErrorMetadata
    {
        // A provider id to relate the execution with an item, for example 'user_id'. useful to match webhook notifications with items.
        [JsonProperty("providerId")]
        public string ProviderId { get; set; }

        // If the connector is MFA, this indicates if MFA credentials are required or not to continue the current execution.
        [JsonProperty("hasMFA")]
        public bool HasMFA { get; set; }

        // Credentials to be used in future executions. May differ or expand from the current execution credentials.
        [JsonProperty("credentials")]
        public object Credentials { get; set; }

        // Device nickname used when device authorization is pending.
        [JsonProperty("deviceNickname")]
        public string DeviceNickname { get; set; }
    }
}
