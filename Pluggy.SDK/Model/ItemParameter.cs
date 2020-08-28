using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ItemParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        public ItemParameter()
        {

        }

        public ItemParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }
    }
}
