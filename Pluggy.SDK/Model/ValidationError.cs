using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ValidationError
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("parameter")]
        public object Parameter { get; set; }

        public ValidationError()
        {

        }

        public ValidationError(string code, string message, string parameter)
        {
            Code = code;
            Message = message;
            Parameter = parameter;
        }
    }
}
