using System.Runtime.Serialization;
using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
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

        [EnumMember(Value = "MOVE_SECURITY")]
        MoveSecurity,
    }
} 