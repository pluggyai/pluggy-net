using System;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecutionResponse
    {
        [JsonProperty("id")]
        public dynamic Id { get; set; }

        [JsonProperty("start_timestamp")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_timestamp")]
        public DateTime? EndTime { get; set; }

        [JsonProperty("finished")]
        public bool Finished { get; set; }

        [JsonProperty("data")]
        public dynamic Data { get; set; }

        [JsonProperty("error")]
        public ExecutionError Error { get; set; }
    }
}
