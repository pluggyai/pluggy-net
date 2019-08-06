using System;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ApiResponse<T> where T : class
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("error")]
        public T Error { get; set; }
    }
}
