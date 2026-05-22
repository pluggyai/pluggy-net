using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Passport
    {
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>Issuing country in alpha3 ISO-3166 format.</summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("issueDate")]
        public DateTime? IssueDate { get; set; }

        [JsonProperty("expirationDate")]
        public DateTime? ExpirationDate { get; set; }
    }
}
