using System;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecutionError
    {
        [JsonProperty("code")]
        public ExecutionErrorCode Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public enum ExecutionErrorCode
    {
        INVALID_CREDENTIALS
    }
}
