using System;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class AuthResponse
    {
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        public AuthResponse()
        {
        }
    }
}
