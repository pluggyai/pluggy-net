using System;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecuteParameter
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        public ExecuteParameter()
        {

        }

        public ExecuteParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
