using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Pluggy.SDK.Errors
{
    /// <summary>
    /// Contains information about an error returned from the API.
    /// </summary>
    //[JsonConverter(typeof(ApiErrorConverter))]
    public class ApiError
    {
        /// <summary>
        /// The error description for the HTTP Status Code
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("errors")]
        public IList<ParameterError> Errors { get; set; }

        /// <summary>
        /// The error code returned by the API
        /// </summary>
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

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


        public bool IsStatus(HttpStatusCode httpStatusCode)
        {
            return StatusCode == (int) httpStatusCode;
        }
    }

}
