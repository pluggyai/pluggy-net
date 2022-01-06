using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ConnectorParameters
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("countries")]
        public List<string> Countries { get; set; }

        [JsonProperty("types")]
        public List<ConnectorType> Types { get; set; }

        [JsonProperty("sandbox")]
        public bool Sandbox { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            var queryStrings = new Dictionary<string, string>()
            {
                { "name", Name },
                { "sandbox", this.Sandbox.ToString().ToLower() }
            };

            // Add each type as a separate querystring
            if (Types != null)
            {
                for (var i = 0; i < Types.Count; i++)
                {
                    queryStrings.Add($"types[{i}]", Types[i].ToString());
                }
            }

            // Add each country as a separate querystring
            if (Countries != null)
            {
                for (var i = 0; i < Countries.Count; i++)
                {
                    queryStrings.Add($"countries[{i}]", Countries[i].ToString());
                }
            }

            return queryStrings;
        }
    }
}
