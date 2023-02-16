using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Email
    {
        [JsonProperty("type")]
        public EmailType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum EmailType
    {
        Personal,
        Work
    }
}
