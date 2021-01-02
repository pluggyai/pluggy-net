using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ConnectTokenResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
    }
}
