using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum InvestmentSubtype
    {
        /*! COE */
        STRUCTURED_NOTE,

        /*! MUTUAL_FUND */
        INVESTMENT_FUND,

        /*! SECURITY */
        RETIREMENT,

        /*! EQUITY */
        STOCK,
        ETF,
        REAL_STATE_FUND,
        /*! BRAZILIAN_DEPOSITARY_RECEIPT */
        BDR,
        DERIVATIVES,

        /*! FIXED_INCOME */
        TREASURY,
        /*! Real State Credit Bill */
        LCI,
        /*! AGRICULTURAL_CREDIT_BILL */
        LCA,
        /*! CERTIFICATE_OF_DEPOSIT */
        CDB,
        /*! REAL_STATE_RECEIVABLE_CERTIFICATE */
        CRI,
        /*! AGRICULTURAL_RECEIVABLE_CERTIFICATE */
        CRA,
        CORPORATE_DEBT,
        /*! BILL_OF_EXCHANGE */
        LC,
        DEBENTURES,
    }
}
