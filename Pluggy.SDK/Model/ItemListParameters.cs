using System.Collections.Generic;

namespace Pluggy.SDK.Model
{
    public class ItemListParameters
    {
        public int? PageSize { get; set; }
        public int? Page { get; set; }

        public IDictionary<string, string> ToQueryStrings()
        {
            var dict = new Dictionary<string, string>();
            if (PageSize.HasValue) dict["pageSize"] = PageSize.Value.ToString();
            if (Page.HasValue) dict["page"] = Page.Value.ToString();
            return dict;
        }
    }
}
