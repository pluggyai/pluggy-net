using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class IncomeReport
    {
        [JsonProperty("year")]
        public int Year { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
