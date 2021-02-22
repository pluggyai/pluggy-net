using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace Pluggy.SDK.Model
{
    public class TransactionParameters
    {
        public TransactionParameters()
        {
        }

        /// <summary>Transaction date from</summary>
        [JsonProperty("from")]
        public DateTime DateFrom { get; set; }

        /// <summary>Transaction date to</summary>
        [JsonProperty("to")]
        public DateTime DateTo { get; set; }

        [JsonProperty("page")]
        public double? Page { get; set; }

        [JsonProperty("size")]
        public double? Size { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>()
            {
                { "to", DateTo.ToString("yyyy-MM-dd") },
                { "from", DateFrom.ToString("yyyy-MM-dd") },
                { "page", Page?.ToString() },
                { "size", Size?.ToString() },
            };
        }
    }
}
