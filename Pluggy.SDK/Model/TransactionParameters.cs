using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionParameters
    {
        public TransactionParameters()
        {
        }

        /// <summary>Transaction date from, format: yyyy-mm-dd</summary>
        [JsonProperty("from")]
        public string DateFrom { get; set; }

        /// <summary>Transaction date to, format: yyyy-mm-dd</summary>
        [JsonProperty("to")]
        public string DateTo { get; set; }

        [JsonProperty("page")]
        public double? Page { get; set; }

        [JsonProperty("size")]
        public double? Size { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>()
            {
                { "to", DateTo },
                { "from", DateFrom },
                { "page", Page?.ToString() },
                { "size", Size?.ToString() },
            };
        }
    }
}
