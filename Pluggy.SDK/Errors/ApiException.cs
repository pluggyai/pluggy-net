using System;
using System.Net;

namespace Pluggy.SDK.Errors
{
    /// <summary>
    /// Represents errors that occur when making API calls.
    /// </summary>
    public class ApiException : Exception
    {
        /// <summary>
        /// The exception payload from the response
        /// </summary>
        public ApiError ApiError { get; }

        /// <summary>
        /// The HTTP status code associated with the response
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="statusCode">The HTTP status code.</param>
        public ApiException(HttpStatusCode statusCode)
            : base(statusCode.ToString())
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="apiError">The API error.</param>
        public ApiException(HttpStatusCode statusCode, ApiError apiError)
            : base(apiError == null ? statusCode.ToString() : apiError.Message)
        {
            StatusCode = statusCode;
            ApiError = apiError;
        }

        /// <summary>
        /// The error code returned by the API (for example <see cref="ApiErrorCode.CLIENT_IP_NOT_ALLOWED"/>),
        /// or null when the response did not include one.
        /// </summary>
        public string ErrorCode => ApiError?.CodeDescription ?? ApiError?.ErrorCode;

        /// <summary>
        /// Whether the API returned the given error code (see <see cref="ApiErrorCode"/>).
        /// </summary>
        /// <param name="code">The error code, for example <see cref="ApiErrorCode.CLIENT_IP_NOT_ALLOWED"/>.</param>
        public bool HasErrorCode(string code)
        {
            return ApiError != null && ApiError.IsCode(code);
        }
    }
}
