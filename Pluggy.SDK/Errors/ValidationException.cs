using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Pluggy.SDK.Errors
{
    public class ValidationException : ApiException
    {
        public List<ParameterError> Errors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="apiError">The API error.</param>
        public ValidationException(HttpStatusCode statusCode, ApiError apiError)
            : base(statusCode, apiError)
        {

        }
    }
}
