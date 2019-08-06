using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecutionResponse
    {
        [JsonProperty("finished")]
        public bool Finished { get; set; }
    }
}
