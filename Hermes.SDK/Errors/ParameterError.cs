using Newtonsoft.Json;

namespace Hermes.SDK.Errors
{
    public class ParameterError
    {
        /// <summary>
        /// The error code of the pararmeter validation error
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// The error message of the pararmeter validation error
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// The parameter's name
        /// </summary>
        [JsonProperty("parameter")]
        public string Parameter { get; set; }

    }
}
