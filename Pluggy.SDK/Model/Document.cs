using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Document
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
