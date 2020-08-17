using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ItemParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public ItemParameter()
        {

        }

        public ItemParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
