using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Pluggy.SDK.Webhooks
{
    /// <summary>
    /// Verifies the <c>X-Pluggy-Signature</c> header of Pluggy webhook notifications.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The header has the form <c>t=&lt;unix timestamp in seconds&gt;,v1=&lt;hex HMAC-SHA256&gt;</c>,
    /// where <c>v1</c> is computed with the signing secret shown in the Pluggy Dashboard (used as-is,
    /// <c>whsec_</c> prefix included) over <c>&lt;t&gt;.&lt;raw request body&gt;</c>. Always verify the
    /// <b>raw</b> body, exactly as received, before any JSON parsing: a re-serialized body does not match.
    /// </para>
    /// <para>
    /// Several secrets can be passed, so the old and the new one are both accepted while a secret is
    /// being rotated. <c>null</c> and empty entries are skipped. Unknown <c>key=value</c> pairs in the
    /// header are ignored, and the signature comparison is constant-time.
    /// </para>
    /// <para>
    /// None of these methods throw on malformed input: <c>Verify</c> returns <c>false</c> and
    /// <c>Validate</c> returns a <see cref="WebhookSignatureResult"/> with the
    /// <see cref="WebhookSignatureFailureReason"/>.
    /// </para>
    /// <para>See https://docs.pluggy.ai/docs/developer-tools/webhook-security</para>
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = WebhookSignature.Verify(
    ///     rawBody,                                       // byte[] or string, before JSON parsing
    ///     request.Headers[WebhookSignature.HeaderName].ToString(),
    ///     Environment.GetEnvironmentVariable("PLUGGY_WEBHOOK_SECRET"),
    ///     Environment.GetEnvironmentVariable("PLUGGY_WEBHOOK_SECRET_OLD")); // unset: skipped
    /// if (!valid)
    /// {
    ///     // answer 401
    /// }
    /// </code>
    /// </example>
    public static class WebhookSignature
    {
        /// <summary>
        /// Name of the HTTP header that carries the webhook signature. HTTP header names are
        /// case-insensitive, and so are the header lookups of ASP.NET Core.
        /// </summary>
        public const string HeaderName = "X-Pluggy-Signature";

        /// <summary>Default maximum age (and clock skew), in seconds, accepted for a signature timestamp.</summary>
        public const long DefaultToleranceInSeconds = 300;

        private const int SignatureBytes = 32;
        // 1 to 12 digits, no sign and no leading zero: covers every timestamp until the year 33658.
        private const int MaxTimestampDigits = 12;

        /// <summary>
        /// Verifies a webhook signature with the default tolerance (5 minutes) and the current time.
        /// </summary>
        /// <param name="payload">The raw request body, exactly as received.</param>
        /// <param name="header">The value of the <c>X-Pluggy-Signature</c> header (may be null).</param>
        /// <param name="secrets">One or more signing secrets; null and empty entries are skipped.</param>
        /// <returns>
        /// <c>true</c> only when the header parses, its timestamp is within the tolerance and the
        /// signature matches one of the secrets.
        /// </returns>
        public static bool Verify(byte[] payload, string header, params string[] secrets)
        {
            return Validate(payload, header, secrets).IsValid;
        }

        /// <summary>
        /// Same as <see cref="Verify(byte[], string, string[])"/>, for a body read as a string.
        /// The string is encoded as UTF-8; it must be the body exactly as received.
        /// </summary>
        public static bool Verify(string payload, string header, params string[] secrets)
        {
            return Validate(payload, header, secrets).IsValid;
        }

        /// <summary>Verifies a webhook signature.</summary>
        /// <param name="payload">The raw request body, exactly as received.</param>
        /// <param name="header">The value of the <c>X-Pluggy-Signature</c> header (may be null).</param>
        /// <param name="secrets">The signing secrets; null and empty entries are skipped.</param>
        /// <param name="toleranceInSeconds">
        /// Maximum difference, in seconds, between the signature timestamp and <paramref name="now"/>,
        /// in either direction (inclusive). A negative value falls back to <see cref="DefaultToleranceInSeconds"/>.
        /// </param>
        /// <param name="now">The current time; null means <see cref="DateTimeOffset.UtcNow"/>.</param>
        /// <returns><c>true</c> only when the signature is valid.</returns>
        public static bool Verify(byte[] payload, string header, IEnumerable<string> secrets,
            long toleranceInSeconds = DefaultToleranceInSeconds, DateTimeOffset? now = null)
        {
            return Validate(payload, header, secrets, toleranceInSeconds, now).IsValid;
        }

        /// <summary>
        /// Same as <see cref="Verify(byte[], string, IEnumerable{string}, long, DateTimeOffset?)"/>,
        /// for a body read as a string (encoded as UTF-8).
        /// </summary>
        public static bool Verify(string payload, string header, IEnumerable<string> secrets,
            long toleranceInSeconds = DefaultToleranceInSeconds, DateTimeOffset? now = null)
        {
            return Validate(payload, header, secrets, toleranceInSeconds, now).IsValid;
        }

        /// <summary>
        /// Validates a webhook signature with the default tolerance (5 minutes) and the current time,
        /// and tells why it was rejected.
        /// </summary>
        /// <param name="payload">The raw request body, exactly as received.</param>
        /// <param name="header">The value of the <c>X-Pluggy-Signature</c> header (may be null).</param>
        /// <param name="secrets">One or more signing secrets; null and empty entries are skipped.</param>
        /// <returns>The result; never null.</returns>
        public static WebhookSignatureResult Validate(byte[] payload, string header, params string[] secrets)
        {
            return Validate(payload, header, (IEnumerable<string>)secrets, DefaultToleranceInSeconds, null);
        }

        /// <summary>
        /// Same as <see cref="Validate(byte[], string, string[])"/>, for a body read as a string
        /// (encoded as UTF-8).
        /// </summary>
        public static WebhookSignatureResult Validate(string payload, string header, params string[] secrets)
        {
            return Validate(payload, header, (IEnumerable<string>)secrets, DefaultToleranceInSeconds, null);
        }

        /// <summary>
        /// Same as <see cref="Validate(byte[], string, IEnumerable{string}, long, DateTimeOffset?)"/>,
        /// for a body read as a string (encoded as UTF-8).
        /// </summary>
        public static WebhookSignatureResult Validate(string payload, string header, IEnumerable<string> secrets,
            long toleranceInSeconds = DefaultToleranceInSeconds, DateTimeOffset? now = null)
        {
            byte[] body;
            try
            {
                body = payload == null ? null : Encoding.UTF8.GetBytes(payload);
            }
            catch (Exception)
            {
                body = null;
            }
            return Validate(body, header, secrets, toleranceInSeconds, now);
        }

        /// <summary>Validates a webhook signature and tells why it was rejected.</summary>
        /// <remarks>
        /// Checks run in this order: <see cref="WebhookSignatureFailureReason.NoSecret"/>,
        /// <see cref="WebhookSignatureFailureReason.InvalidPayload"/>,
        /// <see cref="WebhookSignatureFailureReason.MissingHeader"/>,
        /// <see cref="WebhookSignatureFailureReason.MalformedHeader"/>,
        /// <see cref="WebhookSignatureFailureReason.TimestampOutOfTolerance"/>,
        /// <see cref="WebhookSignatureFailureReason.SignatureMismatch"/>.
        /// </remarks>
        /// <param name="payload">The raw request body, exactly as received.</param>
        /// <param name="header">The value of the <c>X-Pluggy-Signature</c> header (may be null).</param>
        /// <param name="secrets">The signing secrets; null and empty entries are skipped.</param>
        /// <param name="toleranceInSeconds">
        /// Maximum difference, in seconds, between the signature timestamp and <paramref name="now"/>,
        /// in either direction (inclusive). A negative value falls back to <see cref="DefaultToleranceInSeconds"/>.
        /// </param>
        /// <param name="now">The current time; null means <see cref="DateTimeOffset.UtcNow"/>.</param>
        /// <returns>The result; never null.</returns>
        public static WebhookSignatureResult Validate(byte[] payload, string header, IEnumerable<string> secrets,
            long toleranceInSeconds = DefaultToleranceInSeconds, DateTimeOffset? now = null)
        {
            try
            {
                List<string> usableSecrets = UsableSecrets(secrets);
                if (usableSecrets.Count == 0)
                {
                    return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.NoSecret);
                }
                if (payload == null)
                {
                    return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.InvalidPayload);
                }
                if (header == null || header.Trim().Length == 0)
                {
                    return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.MissingHeader);
                }

                ParsedHeader parsed = ParseHeader(header);
                if (parsed == null)
                {
                    return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.MalformedHeader);
                }

                long tolerance = toleranceInSeconds >= 0 ? toleranceInSeconds : DefaultToleranceInSeconds;
                long nowSeconds = (now ?? DateTimeOffset.UtcNow).ToUnixTimeSeconds();
                if (Math.Abs(nowSeconds - parsed.Timestamp) > tolerance)
                {
                    return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.TimestampOutOfTolerance);
                }

                byte[] prefix = Encoding.UTF8.GetBytes(parsed.RawTimestamp + ".");

                // Check every secret, without stopping at the first match, so the time taken does not
                // depend on which secret matched.
                bool matched = false;
                foreach (string secret in usableSecrets)
                {
                    byte[] expected = ComputeSignature(secret, prefix, payload);
                    if (expected.Length == SignatureBytes && FixedTimeEquals(expected, parsed.Signature))
                    {
                        matched = true;
                    }
                }
                return matched
                    ? WebhookSignatureResult.Valid
                    : WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.SignatureMismatch);
            }
            catch (Exception)
            {
                return WebhookSignatureResult.Invalid(WebhookSignatureFailureReason.MalformedHeader);
            }
        }

        private sealed class ParsedHeader
        {
            public ParsedHeader(long timestamp, string rawTimestamp, byte[] signature)
            {
                Timestamp = timestamp;
                RawTimestamp = rawTimestamp;
                Signature = signature;
            }

            public long Timestamp { get; }
            public string RawTimestamp { get; }
            public byte[] Signature { get; }
        }

        private static ParsedHeader ParseHeader(string header)
        {
            string rawTimestamp = null;
            string rawSignature = null;

            // string.Split keeps empty entries, so a trailing comma is rejected as an empty pair.
            foreach (string rawPart in header.Split(','))
            {
                string part = rawPart.Trim();
                int separator = part.IndexOf('=');
                if (separator <= 0)
                {
                    return null;
                }
                string key = part.Substring(0, separator);
                string value = part.Substring(separator + 1);
                if (!IsValidKey(key))
                {
                    return null;
                }
                if (key == "t")
                {
                    if (rawTimestamp != null)
                    {
                        return null;
                    }
                    rawTimestamp = value;
                }
                else if (key == "v1")
                {
                    if (rawSignature != null)
                    {
                        return null;
                    }
                    rawSignature = value;
                }
                // Unknown keys (e.g. a future "v2=") are ignored on purpose.
            }

            if (rawTimestamp == null || rawSignature == null)
            {
                return null;
            }
            if (!IsValidTimestamp(rawTimestamp))
            {
                return null;
            }
            byte[] signature = DecodeHex(rawSignature);
            if (signature == null || signature.Length != SignatureBytes)
            {
                return null;
            }
            long timestamp = long.Parse(rawTimestamp, NumberStyles.None, CultureInfo.InvariantCulture);
            return new ParsedHeader(timestamp, rawTimestamp, signature);
        }

        private static bool IsValidKey(string key)
        {
            if (key.Length == 0)
            {
                return false;
            }
            foreach (char c in key)
            {
                bool ok = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9')
                    || c == '_' || c == '-';
                if (!ok)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsValidTimestamp(string value)
        {
            if (value.Length == 0 || value.Length > MaxTimestampDigits || value[0] == '0')
            {
                return false;
            }
            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }
            return true;
        }

        private static byte[] DecodeHex(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                return null;
            }
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                int high = HexValue(hex[2 * i]);
                int low = HexValue(hex[2 * i + 1]);
                if (high < 0 || low < 0)
                {
                    return null;
                }
                bytes[i] = (byte)((high << 4) | low);
            }
            return bytes;
        }

        private static int HexValue(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return c - '0';
            }
            if (c >= 'a' && c <= 'f')
            {
                return c - 'a' + 10;
            }
            if (c >= 'A' && c <= 'F')
            {
                return c - 'A' + 10;
            }
            return -1;
        }

        private static byte[] ComputeSignature(string secret, byte[] prefix, byte[] payload)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                hmac.TransformBlock(prefix, 0, prefix.Length, null, 0);
                hmac.TransformFinalBlock(payload, 0, payload.Length);
                return hmac.Hash;
            }
        }

        // CryptographicOperations.FixedTimeEquals is not available on .NET Standard 2.0, so this is
        // the same constant-time comparison: the time taken depends only on the length.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }
            int diff = 0;
            for (int i = 0; i < left.Length; i++)
            {
                diff |= left[i] ^ right[i];
            }
            return diff == 0;
        }

        private static List<string> UsableSecrets(IEnumerable<string> secrets)
        {
            var usable = new List<string>();
            if (secrets != null)
            {
                foreach (string secret in secrets)
                {
                    if (!string.IsNullOrEmpty(secret))
                    {
                        usable.Add(secret);
                    }
                }
            }
            return usable;
        }
    }

    /// <summary>Why a webhook signature was rejected.</summary>
    /// <remarks>
    /// Use <see cref="WebhookSignatureFailureReasonExtensions.ToCode"/> to get the same lowercase codes
    /// the other Pluggy SDKs report (<c>no_secret</c>, <c>invalid_payload</c>, ...).
    /// </remarks>
    public enum WebhookSignatureFailureReason
    {
        /// <summary>No usable signing secret was provided (none, or only null / empty entries).</summary>
        [EnumMember(Value = "no_secret")]
        NoSecret,

        /// <summary>The payload is null.</summary>
        [EnumMember(Value = "invalid_payload")]
        InvalidPayload,

        /// <summary>The signature header is absent, empty or blank.</summary>
        [EnumMember(Value = "missing_header")]
        MissingHeader,

        /// <summary>The signature header could not be parsed (bad format, bad timestamp or bad <c>v1</c>).</summary>
        [EnumMember(Value = "malformed_header")]
        MalformedHeader,

        /// <summary>The signature timestamp is too far in the past or in the future.</summary>
        [EnumMember(Value = "timestamp_out_of_tolerance")]
        TimestampOutOfTolerance,

        /// <summary>The signature does not match the payload with any of the given secrets.</summary>
        [EnumMember(Value = "signature_mismatch")]
        SignatureMismatch
    }

    /// <summary>Helpers for <see cref="WebhookSignatureFailureReason"/>.</summary>
    public static class WebhookSignatureFailureReasonExtensions
    {
        /// <summary>
        /// Returns the reason as a lowercase code (<c>no_secret</c>, <c>invalid_payload</c>,
        /// <c>missing_header</c>, <c>malformed_header</c>, <c>timestamp_out_of_tolerance</c>,
        /// <c>signature_mismatch</c>), the same codes the other Pluggy SDKs report.
        /// </summary>
        public static string ToCode(this WebhookSignatureFailureReason reason)
        {
            switch (reason)
            {
                case WebhookSignatureFailureReason.NoSecret:
                    return "no_secret";
                case WebhookSignatureFailureReason.InvalidPayload:
                    return "invalid_payload";
                case WebhookSignatureFailureReason.MissingHeader:
                    return "missing_header";
                case WebhookSignatureFailureReason.MalformedHeader:
                    return "malformed_header";
                case WebhookSignatureFailureReason.TimestampOutOfTolerance:
                    return "timestamp_out_of_tolerance";
                case WebhookSignatureFailureReason.SignatureMismatch:
                    return "signature_mismatch";
                default:
                    return reason.ToString();
            }
        }
    }

    /// <summary>Outcome of <c>WebhookSignature.Validate</c>. Immutable.</summary>
    public sealed class WebhookSignatureResult
    {
        internal static readonly WebhookSignatureResult Valid = new WebhookSignatureResult(null);

        private WebhookSignatureResult(WebhookSignatureFailureReason? reason)
        {
            Reason = reason;
        }

        internal static WebhookSignatureResult Invalid(WebhookSignatureFailureReason reason)
        {
            return new WebhookSignatureResult(reason);
        }

        /// <summary>True when the signature is valid.</summary>
        public bool IsValid => Reason == null;

        /// <summary>Why the signature was rejected, or null when it is valid.</summary>
        public WebhookSignatureFailureReason? Reason { get; }

        /// <summary>
        /// The failure reason as a lowercase code (e.g. <c>signature_mismatch</c>), or null when the
        /// signature is valid.
        /// </summary>
        public string ReasonCode => Reason.HasValue ? Reason.Value.ToCode() : null;

        /// <inheritdoc />
        public override string ToString()
        {
            return IsValid ? "WebhookSignatureResult{valid}" : "WebhookSignatureResult{invalid: " + ReasonCode + "}";
        }
    }
}
