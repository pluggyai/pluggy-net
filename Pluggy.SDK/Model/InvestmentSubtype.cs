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
        MULTIMARKET_FUND,
        FIXED_INCOME_FUND,
        STOCK_FUND,
        ETF_FUND,
        OFFSHORE_FUND,
        FIP_FUND,
        EXCHANGE_FUND,

        /*! SECURITY */
        RETIREMENT,
        VGBL,
        PGBL,

        /*! EQUITY */
        STOCK,
        ETF,
        REAL_ESTATE_FUND,

        /*! BRAZILIAN_DEPOSITARY_RECEIPT */
        BDR,
        DERIVATIVES,
        OPTION,

        /*! FIXED_INCOME */
        TREASURY,
        /*! Real State Credit Bill */
        LCI,
        /*! AGRICULTURAL_CREDIT_BILL */
        LCA,
        /*! CERTIFICATE_OF_DEPOSIT */
        CDB,
        /*! REAL_ESTATE_RECEIVABLE_CERTIFICATE */
        CRI,
        /*! AGRICULTURAL_RECEIVABLE_CERTIFICATE */
        CRA,
        CORPORATE_DEBT,
        /*! BILL_OF_EXCHANGE */
        LC,
        DEBENTURES,
        LIG,
        LF,

        /*! Unknown */
        OTHER,
    }
}
