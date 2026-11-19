using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using NUnit.Framework;
using Pluggy.SDK.Webhooks;

namespace Pluggy.Tests.Webhooks
{
    [TestFixture]
    public class WebhookSignatureTest
    {
        private static readonly string SECRET = "whsec_" + string.Concat(Enumerable.Repeat("a1b2c3d4", 8));
        private static readonly string OTHER_SECRET = "whsec_" + string.Concat(Enumerable.Repeat("f0e1d2c3", 8));
        private const long T = 1763560800;
        private static readonly DateTimeOffset NOW = DateTimeOffset.FromUnixTimeSeconds(T);
        private const string BODY = "{\"event\":\"item/updated\",\"eventId\":\"evt_1\",\"itemId\":\"item_1\"}";

        // Independent reference implementation used to build the headers under test.
        private static string Sign(string secret, string t, byte[] body)
        {
            byte[] prefix = Encoding.UTF8.GetBytes(t + ".");
            byte[] message = prefix.Concat(body).ToArray();
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                return BitConverter.ToString(hmac.ComputeHash(message)).Replace("-", "").ToLowerInvariant();
            }
        }

        private static string Sign(string secret, long t, string body)
        {
            return Sign(secret, t.ToString(), Encoding.UTF8.GetBytes(body));
        }

        private static string Header(long t = T, string body = BODY, string secret = null)
        {
            return "t=" + t + ",v1=" + Sign(secret ?? SECRET, t, body);
        }

        private static WebhookSignatureResult Validate(string header, string payload = BODY,
            IEnumerable<string> secrets = null, long tolerance = WebhookSignature.DefaultToleranceInSeconds)
        {
            return WebhookSignature.Validate(payload, header, secrets ?? new[] { SECRET }, tolerance, NOW);
        }

        private static void AssertValid(WebhookSignatureResult result)
        {
            Assert.IsTrue(result.IsValid, result.ToString());
            Assert.IsNull(result.Reason);
            Assert.IsNull(result.ReasonCode);
        }

        private static void AssertInvalid(WebhookSignatureFailureReason expected, WebhookSignatureResult result)
        {
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expected, result.Reason, result.ToString());
            Assert.AreEqual(expected.ToCode(), result.ReasonCode);
        }

        [Test]
        public void ExposesTheHeaderNameAndDefaultTolerance()
        {
            Assert.AreEqual("X-Pluggy-Signature", WebhookSignature.HeaderName);
            Assert.AreEqual(300, WebhookSignature.DefaultToleranceInSeconds);
        }

        [Test]
        public void MatchesTheSharedKnownAnswerVector()
        {
            string secret = "whsec_" + string.Concat(Enumerable.Repeat("0123456789abcdef", 4));
            string body = "{\"event\":\"item/updated\",\"eventId\":\"evt_test\",\"itemId\":\"item_test\",\"note\":\"ação\"}";
            string v1 = "002c330ef76a9209f11870843509b1ac0f3df0f3b00b0a16712abdd0eff15a34";
            string header = "t=1763560800,v1=" + v1;
            var now = DateTimeOffset.FromUnixTimeSeconds(1763560800);

            Assert.AreEqual(v1, Sign(secret, 1763560800, body));
            AssertValid(WebhookSignature.Validate(body, header, new[] { secret }, 300, now));
            AssertValid(WebhookSignature.Validate(Encoding.UTF8.GetBytes(body), header, new[] { secret }, 300, now));
        }

        [Test]
        public void AcceptsAValidSignature()
        {
            AssertValid(Validate(Header()));
            Assert.IsTrue(WebhookSignature.Verify(BODY, Header(), new[] { SECRET }, 300, NOW));
        }

        [Test]
        public void ParamsOverloadsUseTheCurrentTime()
        {
            long t = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Assert.IsTrue(WebhookSignature.Verify(BODY, Header(t), SECRET));
            Assert.IsTrue(WebhookSignature.Verify(Encoding.UTF8.GetBytes(BODY), Header(t), SECRET));
            AssertValid(WebhookSignature.Validate(BODY, Header(t), OTHER_SECRET, SECRET));
            Assert.IsFalse(WebhookSignature.Verify(BODY, Header(t - 3600), SECRET));
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance,
                WebhookSignature.Validate(Encoding.UTF8.GetBytes(BODY), Header(t - 3600), SECRET));
        }

        [Test]
        public void AcceptsStringAndBytePayloadsAlike()
        {
            AssertValid(WebhookSignature.Validate(Encoding.UTF8.GetBytes(BODY), Header(), new[] { SECRET }, 300, NOW));
            Assert.IsTrue(WebhookSignature.Verify(Encoding.UTF8.GetBytes(BODY), Header(), new[] { SECRET }, 300, NOW));
        }

        [Test]
        public void AcceptsANonAsciiBody()
        {
            string body = "{\"description\":\"Pagamento à vista — café ☕ 日本\"}";
            AssertValid(Validate(Header(T, body), body));
            AssertValid(WebhookSignature.Validate(Encoding.UTF8.GetBytes(body), Header(T, body), new[] { SECRET }, 300, NOW));
        }

        [Test]
        public void AcceptsAnEmptyBody()
        {
            AssertValid(Validate(Header(T, ""), ""));
            AssertValid(WebhookSignature.Validate(new byte[0], Header(T, ""), new[] { SECRET }, 300, NOW));
        }

        [Test]
        public void RejectsABodyAlteredByOneByte()
        {
            byte[] altered = Encoding.UTF8.GetBytes(BODY);
            altered[altered.Length - 2] ^= 0x01;
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch,
                WebhookSignature.Validate(altered, Header(), new[] { SECRET }, 300, NOW));
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch, Validate(Header(), BODY + " "));
        }

        [Test]
        public void RejectsAReserializedBody()
        {
            string reserialized = BODY.Replace(",", ", ");
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch, Validate(Header(), reserialized));
        }

        [Test]
        public void RejectsANullPayload()
        {
            AssertInvalid(WebhookSignatureFailureReason.InvalidPayload,
                WebhookSignature.Validate((string)null, Header(), new[] { SECRET }, 300, NOW));
            AssertInvalid(WebhookSignatureFailureReason.InvalidPayload,
                WebhookSignature.Validate((byte[])null, Header(), new[] { SECRET }, 300, NOW));
            Assert.IsFalse(WebhookSignature.Verify((byte[])null, Header(), SECRET));
        }

        [Test]
        public void RejectsAWrongSecret()
        {
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch, Validate(Header(), secrets: new[] { OTHER_SECRET }));
        }

        [Test]
        public void UsesTheWholeSecretPrefixIncluded()
        {
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch,
                Validate(Header(), secrets: new[] { SECRET.Replace("whsec_", "") }));
        }

        [Test]
        public void AcceptsAnySecretOfAList()
        {
            AssertValid(Validate(Header(), secrets: new[] { OTHER_SECRET, SECRET }));
            AssertValid(Validate(Header(), secrets: new[] { SECRET, OTHER_SECRET }));
            AssertValid(Validate(Header(), secrets: new List<string> { OTHER_SECRET, SECRET }));
            AssertInvalid(WebhookSignatureFailureReason.SignatureMismatch,
                Validate(Header(), secrets: new[] { OTHER_SECRET, "", "whsec_x" }));
        }

        [Test]
        public void SkipsNullAndEmptySecrets()
        {
            AssertValid(Validate(Header(), secrets: new[] { null, SECRET, "" }));
            AssertValid(WebhookSignature.Validate(BODY, Header(), new[] { null, SECRET }, 300, NOW));
        }

        [Test]
        public void ReportsNoSecretWhenThereIsNoUsableSecret()
        {
            AssertInvalid(WebhookSignatureFailureReason.NoSecret, Validate(Header(), secrets: new string[0]));
            AssertInvalid(WebhookSignatureFailureReason.NoSecret, Validate(Header(), secrets: new[] { "" }));
            AssertInvalid(WebhookSignatureFailureReason.NoSecret, Validate(Header(), secrets: new string[] { null, "" }));
            AssertInvalid(WebhookSignatureFailureReason.NoSecret,
                WebhookSignature.Validate(BODY, Header(), (IEnumerable<string>)null, 300, NOW));
            AssertInvalid(WebhookSignatureFailureReason.NoSecret, WebhookSignature.Validate(BODY, Header()));
            AssertInvalid(WebhookSignatureFailureReason.NoSecret, WebhookSignature.Validate(BODY, Header(), (string[])null));
        }

        [Test]
        public void ChecksRunInTheDocumentedOrder()
        {
            // no_secret wins over everything else
            AssertInvalid(WebhookSignatureFailureReason.NoSecret,
                WebhookSignature.Validate((byte[])null, null, new string[0], 300, NOW));
            // then invalid_payload
            AssertInvalid(WebhookSignatureFailureReason.InvalidPayload,
                WebhookSignature.Validate((byte[])null, null, new[] { SECRET }, 300, NOW));
            // then missing_header
            AssertInvalid(WebhookSignatureFailureReason.MissingHeader, Validate(null));
            // then malformed_header
            AssertInvalid(WebhookSignatureFailureReason.MalformedHeader, Validate("t=" + (T - 3600)));
            // then timestamp_out_of_tolerance, even with a wrong signature
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance,
                Validate(Header(T - 3600, BODY, OTHER_SECRET)));
        }

        [Test]
        public void RejectsATimestampTooOld()
        {
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance, Validate(Header(T - 301)));
        }

        [Test]
        public void RejectsATimestampTooFarInTheFuture()
        {
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance, Validate(Header(T + 301)));
        }

        [Test]
        public void AcceptsBothBoundariesInclusive()
        {
            AssertValid(Validate(Header(T - 300)));
            AssertValid(Validate(Header(T + 300)));
        }

        [Test]
        public void HonoursACustomTolerance()
        {
            AssertValid(Validate(Header(T - 10), tolerance: 10));
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance, Validate(Header(T - 11), tolerance: 10));
            AssertValid(Validate(Header(T), tolerance: 0));
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance, Validate(Header(T - 1), tolerance: 0));
        }

        [TestCase(-1)]
        [TestCase(long.MinValue)]
        public void FallsBackToTheDefaultForANegativeTolerance(long tolerance)
        {
            AssertValid(Validate(Header(T - 300), tolerance: tolerance));
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance, Validate(Header(T - 301), tolerance: tolerance));
        }

        [Test]
        public void IgnoresTheSubSecondPartOfNow()
        {
            var now = NOW.AddMilliseconds(999);
            AssertValid(WebhookSignature.Validate(BODY, Header(T - 300), new[] { SECRET }, 300, now));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void ReportsAMissingOrEmptyHeader(string header)
        {
            AssertInvalid(WebhookSignatureFailureReason.MissingHeader, Validate(header));
            Assert.IsFalse(WebhookSignature.Verify(BODY, header, SECRET));
        }

        [Test]
        public void ToleratesWhitespaceAroundPairsAndAnyPairOrder()
        {
            string v1 = Sign(SECRET, T, BODY);
            AssertValid(Validate(" t=" + T + " , v1=" + v1 + " "));
            AssertValid(Validate("v1=" + v1 + ",t=" + T));
        }

        [Test]
        public void AcceptsAnUppercaseHexSignature()
        {
            AssertValid(Validate("t=" + T + ",v1=" + Sign(SECRET, T, BODY).ToUpperInvariant()));
        }

        [Test]
        public void IgnoresUnknownPairs()
        {
            string v1 = Sign(SECRET, T, BODY);
            AssertValid(Validate("t=" + T + ",v1=" + v1 + ",v2=" + new string('b', 128)));
            AssertValid(Validate("v0=abc,t=" + T + ",foo=bar=baz,v1=" + v1));
        }

        private static IEnumerable<TestCaseData> MalformedHeaders()
        {
            string v1 = Sign(SECRET, T, BODY);
            byte[] v1Bytes = Enumerable.Range(0, 32).Select(i => Convert.ToByte(v1.Substring(i * 2, 2), 16)).ToArray();
            yield return new TestCaseData("garbage").SetName("NoPairs");
            yield return new TestCaseData("t=" + T).SetName("OnlyT");
            yield return new TestCaseData("v1=" + v1).SetName("OnlyV1");
            yield return new TestCaseData("t=" + T + ",,v1=" + v1).SetName("EmptyPair");
            yield return new TestCaseData("t=" + T + ",v1=" + v1 + ",").SetName("TrailingComma");
            yield return new TestCaseData("t=" + T + ",=x,v1=" + v1).SetName("PairWithoutKey");
            yield return new TestCaseData("t=" + T + ",v 1=" + v1).SetName("KeyWithWhitespace");
            yield return new TestCaseData("t=" + T + ",t=" + T + ",v1=" + v1).SetName("DuplicateT");
            yield return new TestCaseData("t=" + T + ",v1=" + v1 + ",v1=" + v1).SetName("DuplicateV1");
            yield return new TestCaseData("t=,v1=" + v1).SetName("EmptyT");
            yield return new TestCaseData("t=-" + T + ",v1=" + v1).SetName("NegativeT");
            yield return new TestCaseData("t=+" + T + ",v1=" + v1).SetName("SignedT");
            yield return new TestCaseData("t=" + T + ".5,v1=" + v1).SetName("DecimalT");
            yield return new TestCaseData("t=1.7e9,v1=" + v1).SetName("ExponentT");
            yield return new TestCaseData("t=0x" + T.ToString("x") + ",v1=" + v1).SetName("HexT");
            yield return new TestCaseData("t=0" + T + ",v1=" + v1).SetName("LeadingZeroT");
            yield return new TestCaseData("t=0,v1=" + v1).SetName("ZeroT");
            yield return new TestCaseData("t=" + new string('9', 13) + ",v1=" + v1).SetName("AbsurdT");
            yield return new TestCaseData("t= " + T + ",v1=" + v1).SetName("TWithInnerWhitespace");
            yield return new TestCaseData("t=" + T + "١,v1=" + v1).SetName("TWithNonAsciiDigit");
            yield return new TestCaseData("t=" + (T * 1000) + ",v1=" + v1).SetName("MillisecondsT");
            yield return new TestCaseData("t=" + T + ",v1=").SetName("EmptyV1");
            yield return new TestCaseData("t=" + T + ",v1=" + new string('z', 64)).SetName("NonHexV1");
            yield return new TestCaseData("t=" + T + ",v1=" + v1.Substring(0, 62)).SetName("ShortV1");
            yield return new TestCaseData("t=" + T + ",v1=" + v1 + "00").SetName("LongV1");
            yield return new TestCaseData("t=" + T + ",v1=" + v1.Substring(0, 63)).SetName("OddLengthV1");
            yield return new TestCaseData("t=" + T + ",v1=" + Convert.ToBase64String(v1Bytes)).SetName("Base64V1");
            yield return new TestCaseData("t=" + T + ",v1= " + v1).SetName("V1WithInnerWhitespace");
        }

        [TestCaseSource(nameof(MalformedHeaders))]
        public void RejectsAMalformedHeader(string header)
        {
            AssertInvalid(WebhookSignatureFailureReason.MalformedHeader, Validate(header));
            Assert.IsFalse(WebhookSignature.Verify(BODY, header, new[] { SECRET }, 300, NOW));
        }

        [Test]
        public void NeverThrowsOnHostileInput()
        {
            var inputs = new[]
            {
                new string('x', 100000),
                string.Join(",", Enumerable.Repeat("a=b", 10000)),
                "t=" + T + ",v1=\ud800",
                "\0",
            };
            foreach (string header in inputs)
            {
                WebhookSignatureResult result = null;
                Assert.DoesNotThrow(() => result = Validate(header));
                Assert.IsFalse(result.IsValid);
            }
            Assert.DoesNotThrow(() => WebhookSignature.Verify("\ud800", Header(), SECRET));
            Assert.DoesNotThrow(() => WebhookSignature.Validate(BODY, Header(), new[] { SECRET }, 300, DateTimeOffset.MinValue));
            Assert.DoesNotThrow(() => WebhookSignature.Validate(BODY, Header(), new[] { SECRET }, long.MaxValue, DateTimeOffset.MaxValue));
            AssertInvalid(WebhookSignatureFailureReason.TimestampOutOfTolerance,
                WebhookSignature.Validate(BODY, Header(), new[] { SECRET }, 300, DateTimeOffset.MinValue));
        }

        [Test]
        public void ExposesTheSameReasonCodesAsTheOtherSdks()
        {
            Assert.AreEqual("no_secret", WebhookSignatureFailureReason.NoSecret.ToCode());
            Assert.AreEqual("invalid_payload", WebhookSignatureFailureReason.InvalidPayload.ToCode());
            Assert.AreEqual("missing_header", WebhookSignatureFailureReason.MissingHeader.ToCode());
            Assert.AreEqual("malformed_header", WebhookSignatureFailureReason.MalformedHeader.ToCode());
            Assert.AreEqual("timestamp_out_of_tolerance", WebhookSignatureFailureReason.TimestampOutOfTolerance.ToCode());
            Assert.AreEqual("signature_mismatch", WebhookSignatureFailureReason.SignatureMismatch.ToCode());
            Assert.AreEqual("WebhookSignatureResult{invalid: signature_mismatch}",
                Validate(Header(), secrets: new[] { OTHER_SECRET }).ToString());
            Assert.AreEqual("WebhookSignatureResult{valid}", Validate(Header()).ToString());
        }
    }
}
