using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ConnectorFilters
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("countries")]
        public List<string> Countries { get; set; }

        [JsonProperty("types")]
        public string Types { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>()
            {
                { "name", Name },
                { "countries", string.Join(",", Countries)},
                { "types", Types },
            };
        }
    }
}
