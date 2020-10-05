using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class IdentityRelation
    {
        [JsonProperty("type")]
        public IdentityRelationType Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }
    }

    public enum IdentityRelationType
    {
        Mother,
        Father,
        Spouse
    }
}
