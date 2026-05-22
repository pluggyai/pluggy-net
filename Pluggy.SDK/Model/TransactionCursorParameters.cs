using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class TransactionCursorParameters
    {
        public TransactionCursorParameters()
        {
        }

        /// <summary>Filter transactions with date >= this value. Format yyyy-MM-dd. Mutually exclusive with CreatedAtFrom.</summary>
        [JsonProperty("dateFrom")]
        public DateTime? DateFrom { get; set; }

        /// <summary>Filter transactions with date <= this value. Format yyyy-MM-dd.</summary>
        [JsonProperty("dateTo")]
        public DateTime? DateTo { get; set; }

        /// <summary>Filter transactions created at or after this timestamp. Mutually exclusive with DateFrom.</summary>
        [JsonProperty("createdAtFrom")]
        public DateTime? CreatedAtFrom { get; set; }

        /// <summary>Base64-encoded cursor from the previous page's Next field.</summary>
        [JsonProperty("after")]
        public string After { get; set; }

        /// <summary>Filter transactions with the specified ids. Max 500 ids per request.</summary>
        [JsonProperty("ids")]
        public IList<string> Ids { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            var qs = new Dictionary<string, string>
            {
                { "dateFrom", DateFrom.HasValue ? DateFrom.Value.ToString("yyyy-MM-dd") : null },
                { "dateTo", DateTo.HasValue ? DateTo.Value.ToString("yyyy-MM-dd") : null },
                { "createdAtFrom", CreatedAtFrom.HasValue ? CreatedAtFrom.Value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") : null },
                { "after", After },
                { "ids", Ids != null && Ids.Count > 0 ? string.Join(",", Ids) : null },
            };
            return qs;
        }
    }
}
