using System;
using System.Net;

namespace Hermes.SDK.Errors
{
    public class NotFoundException : ApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="apiError">The API error.</param>
        public NotFoundException(HttpStatusCode statusCode, ApiError apiError)
            : base(statusCode, apiError)
        {

        }
    }
}
