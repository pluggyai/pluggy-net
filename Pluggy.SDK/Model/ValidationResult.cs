using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Pluggy.SDK.Model
{
    public class ValidationResult
    {
        [JsonProperty("parameters")]
        public List<ItemParameter> Parameters { get; set; }
        
        [JsonProperty("errors")]
        public List<ValidationError> Errors { get; set; }

        public ValidationResult()
        {
        }

        public ValidationResult(List<ItemParameter> parameters, List<ValidationError> errors)
        {
            this.Parameters = parameters;
            this.Errors = errors;
        }

        public IDictionary<string, object> ToBody()
        {
            return new Dictionary<string, object>()
            {
                { "parameters", Parameters?.ToDictionary(x => x.Name, x => x.Value) },
                { "errors", Errors },
            };
        }
    }
}
