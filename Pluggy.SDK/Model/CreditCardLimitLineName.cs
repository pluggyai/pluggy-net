using Newtonsoft.Json;
using Pluggy.SDK.Utils;

namespace Pluggy.SDK.Model
{
    [JsonConverter(typeof(TolerantEnumConverter))]
    public enum CreditCardLimitLineName
    {
        CREDITO_A_VISTA,
        CREDITO_PARCELADO,
        SAQUE_CREDITO_BRASIL,
        SAQUE_CREDITO_EXTERIOR,
        EMPRESTIMO_CARTAO_CONSIGNADO,
        OUTROS,
    }
}
