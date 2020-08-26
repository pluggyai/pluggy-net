using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Category
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("parentDescription")]
        public string ParentDescription { get; set; }
    }
}
