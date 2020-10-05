using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class PhoneNumber
    {
        [JsonProperty("type")]
        public PhoneNumberType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum PhoneNumberType
    {
        Personal,
        Work,
        Residencial
    }
}
