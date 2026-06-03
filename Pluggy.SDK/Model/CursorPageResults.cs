using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class CursorPageResults<TResult>
    {
        [JsonProperty("results")]
        public IList<TResult> Results { get; set; }

        /// <summary>Query string for the next page of results. Null if there are no more results.</summary>
        [JsonProperty("next")]
        public string Next { get; set; }

        public CursorPageResults()
        {
        }
    }
}
