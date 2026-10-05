using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// Coupon-payment schedule for coupon-bearing fixed income and Treasury bonds.
    /// </summary>
    public class InvestmentCouponPayment
    {
        /// <summary>Whether the paper pays periodic coupons.</summary>
        [JsonProperty("hasCoupon")]
        public bool? HasCoupon { get; set; }

        /// <summary>
        /// Frequency of coupon payments: MONTHLY, QUARTERLY, SEMESTERLY, YEARLY or IRREGULAR.
        /// </summary>
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }

        /// <summary>Free-text detail when <see cref="Periodicity"/> is IRREGULAR.</summary>
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }
}
