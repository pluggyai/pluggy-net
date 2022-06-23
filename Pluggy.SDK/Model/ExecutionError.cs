using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ExecutionError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
    
	public static class ExecutionErrorCode
    {
        public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
        public const string INVALID_CREDENTIALS_MFA = "INVALID_CREDENTIALS_MFA";
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
}
