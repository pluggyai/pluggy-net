using System;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// A scheduled payment belonging to a payment request.
    /// </summary>
    public class SchedulePayment
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public SchedulePaymentStatus Status { get; set; }

        [JsonProperty("scheduledDate")]
        public DateTime? ScheduledDate { get; set; }

        [JsonProperty("endToEndId")]
        public string EndToEndId { get; set; }

        [JsonProperty("errorDetail")]
        public SchedulePaymentErrorDetail ErrorDetail { get; set; }
    }

    public class SchedulePaymentErrorDetail
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }
    }

    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum SchedulePaymentStatus
    {
        SCHEDULED,
        COMPLETED,
        ERROR
    }
}
