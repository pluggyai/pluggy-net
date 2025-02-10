using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProductType
    {
        [EnumMember(Value = "ACCOUNTS")]
        Accounts,
        
        [EnumMember(Value = "CREDIT_CARDS")]
        CreditCards,
        
        [EnumMember(Value = "TRANSACTIONS")]
        Transactions,
        
        [EnumMember(Value = "PAYMENT_DATA")]
        PaymentData,
        
        [EnumMember(Value = "INVESTMENTS")]
        Investments,
        
        [EnumMember(Value = "INVESTMENTS_TRANSACTIONS")]
        InvestmentsTransactions,
        
        [EnumMember(Value = "BROKERAGE_NOTE")]
        BrokerageNote,
        
        [EnumMember(Value = "LOANS")]
        Loans,
        
        [EnumMember(Value = "EXCHANGE_OPERATIONS")]
        ExchangeOperations,
        
        [EnumMember(Value = "IDENTITY")]
        Identity,

        [EnumMember(Value = "OPPORTUNITIES")]
        Opportunities,

        [EnumMember(Value = "PORTFOLIO")]
        Portfolio,

        [EnumMember(Value = "INCOME_REPORTS")]
        IncomeReports,

        [EnumMember(Value = "MOVE_SECURITY")]
        MoveSecurity,

        [EnumMember(Value = "ACQUIRER_OPERATIONS")]
        AcquirerOperations,

        [EnumMember(Value = "BENEFITS")]
        Benefits,
    }
} 