using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class WebhookEvent
    {
        public static WebhookEvent ITEM_CREATED { get { return new WebhookEvent("item/created"); } }
        public static WebhookEvent ITEM_UPDATED { get { return new WebhookEvent("item/updated"); } }
        public static WebhookEvent ITEM_DELETED { get { return new WebhookEvent("item/deleted"); } }
        public static WebhookEvent ITEM_ERROR { get { return new WebhookEvent("item/error"); } }
        public static WebhookEvent ITEM_LOGIN_SUCCEEDED { get { return new WebhookEvent("item/login_succeeded"); } }
        public static WebhookEvent ITEM_WAITING_USER_INPUT { get { return new WebhookEvent("item/waiting_user_input"); } }
        public static WebhookEvent CONNECTOR_STATUS_UPDATED { get { return new WebhookEvent("connector/status_updated"); } }
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

        [JsonProperty("disabledAt")]
        public DateTime? DisabledAt { get; set; }
    }

    public class WebhookEventPayload
    {
        [Obsolete("Use EventId instead", false)]
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("eventId")]
        public Guid EventId { get; set; }
    
        [JsonProperty("itemId")]
        public Guid ItemId { get; set; }

        [JsonProperty("event")]
        private readonly string _event;

        public WebhookEvent Event
        {
            get { return new WebhookEvent(_event); }
        }

        [JsonProperty("data")]
        public WebhookData Data { get; set; }

        [JsonProperty("error")]
        public ExecutionError Error { get; set; }
    }
}
