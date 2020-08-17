using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class Item
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("status")]
        public ItemStatus Status { get; set; }

        [JsonProperty("executionStatus")]
        public string ExecutionStatus { get; set; }

        [JsonProperty("error")]
        public ExecutionError Error { get; set; }


        public Boolean HasFinished ()
        {
            return this.Status != ItemStatus.OUTDATED || this.Status != ItemStatus.OUTDATED;
        }
    }
}
