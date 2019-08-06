using System;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class RobotParameter
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public RobotParameter()
        {

        }
    }
}
