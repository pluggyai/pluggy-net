using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ItemResourceParameters
    {
        public ItemResourceParameters()
        {
        }

        /// <summary>Page number, default 1.</summary>
        [JsonProperty("page")]
        public int? Page { get; set; }

        /// <summary>Page size, default 500.</summary>
        [JsonProperty("pageSize")]
        public int? PageSize { get; set; }

        /// <summary>Only resources with this status, one of <see cref="ItemResourceStatus"/>.</summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>
            {
                { "page", Page?.ToString() },
                { "pageSize", PageSize?.ToString() },
                { "status", Status },
            };
        }
    }
}
