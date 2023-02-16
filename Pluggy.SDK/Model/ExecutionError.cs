using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ExecutionError
    {
        // The specific execution error code.
        [JsonProperty("code")]
        public string Code { get; set; }

        // A human-readable, short description of the error
        [JsonProperty("message")]
        public string Message { get; set; }

        // The exact error message returned by the institution, if any was provided.
        [JsonProperty("providerMessage")]
        public string ProviderMessage { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        // Unstructured properties that provide additional context/information of the error.
        // Used for some specific cases only, such as Caixa PF & PJ.
        // see https://docs.pluggy.ai/docs/errors-validations for more info.
        [JsonProperty("attributes")]
        public object Attributes { get; set; }
    }
    
	public static class ExecutionErrorCode
    {
        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string INVALID_CREDENTIALS_MFA = "INVALID_CREDENTIALS_MFA";
        public const string ACCOUNT_CREDENTIALS_RESET = "ACCOUNT_CREDENTIALS_RESET";
        public const string ALREADY_LOGGED_IN = "ALREADY_LOGGED_IN";
        public const string ACCOUNT_LOCKED = "ACCOUNT_LOCKED";
        public const string SITE_NOT_AVAILABLE = "SITE_NOT_AVAILABLE";
        public const string UNEXPECTED_ERROR = "UNEXPECTED_ERROR";
        public const string ACCOUNT_NEEDS_ACTION = "ACCOUNT_NEEDS_ACTION";
        public const string CONNECTION_ERROR = "CONNECTION_ERROR";
        public const string USER_AUTHORIZATION_PENDING = "USER_AUTHORIZATION_PENDING";
        public const string USER_AUTHORIZATION_NOT_GRANTED = "USER_AUTHORIZATION_NOT_GRANTED";
        public const string USER_INPUT_TIMEOUT = "USER_INPUT_TIMEOUT";
    }

    public static class Metadata
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
