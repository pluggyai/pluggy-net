using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class Robot
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("credentials")]
        public IList<RobotParameter> Credentials { get; set; }

        public Robot()
        {

        }
    }
}
