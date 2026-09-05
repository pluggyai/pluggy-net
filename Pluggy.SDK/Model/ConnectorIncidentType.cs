using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    /// <summary>
    /// What is broken, as opposed to how badly (severity) or how far along Pluggy
    /// is (state). Use it to group the same problem across institutions, and to
    /// decide which incidents are worth showing your own users: a
    /// SCHEDULED_MAINTENANCE and a TRANSACTIONS_MISSING both read as "degraded"
    /// otherwise.
    ///
    /// The converter falls back to OTHER, so a value Pluggy adds later
    /// deserializes as "not classified" on an older SDK rather than silently
    /// taking the meaning of whichever value happens to be declared first.
    /// </summary>
    [JsonConverter(typeof(TolerantEnumConverter), "OTHER")]
    public enum ConnectorIncidentType
    {
        // Whether you can connect at all.
        CONNECTOR_UNAVAILABLE,
        CONNECTOR_DEGRADED,
        INSTITUTION_OUTAGE,
        SCHEDULED_MAINTENANCE,

        // It answers, but the connection does not complete or does not refresh.
        CONSENT_ERROR,
        CONNECTION_NOT_UPDATING,
        PARTIAL_SUCCESS,

        // It connects and syncs, but what comes back is wrong or incomplete.
        ACCOUNTS_MISSING,
        BALANCE_INCORRECT,
        TRANSACTIONS_MISSING,
        TRANSACTIONS_INCORRECT,
        TRANSACTIONS_INSTALLMENTS_ISSUE,
        INVESTMENTS_MISSING,
        INVESTMENTS_INCORRECT,
        IDENTITY_MISSING,
        HISTORICAL_DATA_MISSING,

        // Pluggy's own platform, not the institution's.
        WEBHOOK_DELAY,
        PAYMENT_FAILURE,

        /// <summary>Pluggy has not classified the incident — not that nothing is wrong.</summary>
        OTHER,
    }
}
