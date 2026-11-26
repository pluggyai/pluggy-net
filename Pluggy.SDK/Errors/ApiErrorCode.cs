namespace Pluggy.SDK.Errors
{
    /// <summary>
    /// Known values of <see cref="ApiError.CodeDescription"/>, the error code the API returns in the
    /// <c>codeDescription</c> field. See https://docs.pluggy.ai/reference/error-codes
    /// </summary>
    public static class ApiErrorCode
    {
        /// <summary>
        /// HTTP 403: the request came from an IP address that is not in the list of allowed IPs configured
        /// for the application in the Dashboard. Returned by <c>POST /auth</c> and by every request made with
        /// the application's API key. See https://docs.pluggy.ai/docs/security/ip-allowlist
        /// </summary>
        public const string CLIENT_IP_NOT_ALLOWED = "CLIENT_IP_NOT_ALLOWED";
    }
}
