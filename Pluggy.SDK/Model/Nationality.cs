using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Nationality
    {
        [JsonProperty("hasBrazilianNationality")]
        public bool? HasBrazilianNationality { get; set; }

        [JsonProperty("otherNationalities")]
        public List<OtherNationality> OtherNationalities { get; set; }
    }
}
