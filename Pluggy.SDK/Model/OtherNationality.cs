using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class OtherNationality
    {
        /// <summary>Country code in alpha3 ISO-3166 format.</summary>
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("documents")]
        public List<NationalityDocument> Documents { get; set; }
    }
}
