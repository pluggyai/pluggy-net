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

        [JsonProperty("pageSize")]
        public double? PageSize { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>()
            {
                // An unset DateTime is 0001-01-01: send no filter rather than that date.
                { "to", DateTo == default ? null : DateTo.ToString("yyyy-MM-dd") },
                { "from", DateFrom == default ? null : DateFrom.ToString("yyyy-MM-dd") },
                { "page", Page?.ToString() },
                { "pageSize", PageSize?.ToString() },
            };
        }
    }
}
