using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class WebhookEvent
    {
        public static WebhookEvent ITEM_CREATED { get { return new WebhookEvent("item/created"); } }
        public static WebhookEvent ITEM_UPDATED { get { return new WebhookEvent("item/updated"); } }
        public static WebhookEvent ITEM_ERROR { get { return new WebhookEvent("item/error"); } }
        public static WebhookEvent ITEM_ALL { get { return new WebhookEvent("all"); } }

        public string Value { get; set; }

        public WebhookEvent(string value)
        {
            Value = value;
        }
    }

    public class Webhook
    {

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("event")]
        private readonly string _event;

        public WebhookEvent Event
        {
            get { return new WebhookEvent(_event); }
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

    }
}
