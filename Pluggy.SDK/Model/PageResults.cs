using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class PageResults<TResult>
    {
        [JsonProperty("results")]
        public IList<TResult> Results { get; set; }

        [JsonProperty("total")]
        public int? Total { get; set; }

        [JsonProperty("totalPages")]
        public int? TotalPages { get; set; }

        [JsonProperty("page")]
        public int? PageNr { get; set; }


        public PageResults()
        {
        }
    }
}
