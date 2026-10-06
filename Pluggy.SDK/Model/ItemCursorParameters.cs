using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Filters for listing items with GET /v2/items (<c>FetchItemsCursor</c> / <c>FetchAllItems</c>).
    /// </summary>
    /// <remarks>
    /// Opt-in, paid plans only. Listing items is disabled by default and is only available to paid-plan teams
    /// that have explicitly requested it from Pluggy support. Teams without it enabled get
    /// 403 LIST_ITEMS_FEATURE_NOT_ENABLED. For most integrations, store each itemId when it is created
    /// (Pluggy Connect onSuccess or the item/created webhook) and use FetchItem(id) instead.
    /// </remarks>
    public class ItemCursorParameters
    {
        public ItemCursorParameters()
        {
        }

        /// <summary>Only items created with this clientUserId. Max 255 characters.</summary>
        [JsonProperty("clientUserId")]
        public string ClientUserId { get; set; }

        /// <summary>Only items of this connector.</summary>
        [JsonProperty("connectorId")]
        public int? ConnectorId { get; set; }

        /// <summary>Opaque cursor taken from the previous page's Next field.</summary>
        [JsonProperty("after")]
        public string After { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            return new Dictionary<string, string>
            {
                { "clientUserId", ClientUserId },
                { "connectorId", ConnectorId?.ToString() },
                { "after", After },
            };
        }
    }
}
