using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Pluggy.SDK.Errors
{
    /// <summary>
    /// Contains information about an error returned from the API.
    /// </summary>
    /// <remarks>
    /// The API returns errors as <c>{ "code": 403, "codeDescription": "CLIENT_IP_NOT_ALLOWED", "message": "..." }</c>.
    /// <see cref="Code"/> and <see cref="CodeDescription"/> map those fields. <see cref="StatusCode"/> and
    /// <see cref="ErrorCode"/> are kept for compatibility and carry the same values.
    /// </remarks>
    //[JsonConverter(typeof(ApiErrorConverter))]
    public class ApiError
    {
        /// <summary>
        /// The error description for the HTTP Status Code
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        /// <summary>
        /// When there are item validations, it will include the reason for each param
        /// </summary>
        [JsonProperty("details")]
        public IList<ParameterError> Details { get; set; }

        /// <summary>
        /// The error code returned by the API (for example <c>CLIENT_IP_NOT_ALLOWED</c>).
        /// Same value as <see cref="CodeDescription"/>; kept for compatibility.
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        /// <summary>
        /// The error code returned by the API in the <c>codeDescription</c> field
        /// (for example <c>CLIENT_IP_NOT_ALLOWED</c>). See <see cref="ApiErrorCode"/> for known values.
        /// </summary>
        [JsonProperty("codeDescription")]
        public string CodeDescription { get; set; }

        /// <summary>
        /// The numeric code returned by the API in the <c>code</c> field (the HTTP status of the error),
        /// or null when the response body did not include it.
        /// </summary>
        [JsonIgnore]
        public int? Code { get; set; }

        /// <summary>
        /// The desription for the error.
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// The HTTP Status code.
        /// </summary>
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        // "code" is a number in the API error body; parsed leniently so that an unexpected type
        // never makes the whole error body unreadable.
        [JsonProperty("code")]
        private object RawCode
        {
            get { return Code; }
            set { Code = ParseCode(value); }
        }

        public bool IsStatus(HttpStatusCode httpStatusCode)
        {
            return StatusCode == (int) httpStatusCode;
        }

        /// <summary>
        /// Whether the API returned the given error code (see <see cref="ApiErrorCode"/>).
        /// </summary>
        /// <param name="code">The error code, for example <see cref="ApiErrorCode.CLIENT_IP_NOT_ALLOWED"/>.</param>
        public bool IsCode(string code)
        {
            return code != null && (code == CodeDescription || code == ErrorCode);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            if (string.IsNullOrEmpty(CodeDescription))
                CodeDescription = ErrorCode;
            if (string.IsNullOrEmpty(ErrorCode))
                ErrorCode = CodeDescription;
            if (StatusCode == 0 && Code.HasValue)
                StatusCode = Code.Value;
        }

        private static int? ParseCode(object value)
        {
            switch (value)
            {
                case null:
                    return null;
                case long l when l >= int.MinValue && l <= int.MaxValue:
                    return (int) l;
                case int i:
                    return i;
                case string s when int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed):
                    return parsed;
                default:
                    return null;
            }
        }
    }

}
