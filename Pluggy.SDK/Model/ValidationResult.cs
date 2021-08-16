using System.Collections.Generic;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ValidationResult
    {
        [JsonProperty("parameters")]
        public Dictionary<string, string> Parameters { get; set; }
        
        [JsonProperty("errors")]
        public List<ValidationError> Errors { get; set; }

        public ValidationResult()
        {
        }
    }
}
