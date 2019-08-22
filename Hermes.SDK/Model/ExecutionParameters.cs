using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecutionParameters
    {
        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("endDate")]
        public string EndDate { get; set; }

        [JsonProperty("credentials")]
        public List<ExecuteParameter> Credentials { get; set; }

        public ExecutionParameters()
        {
        }

        public ExecutionParameters(List<ExecuteParameter> credentials, DateTime? startDate = null, DateTime? endDate = null)
        {
            this.Credentials = credentials;
            this.StartDate = startDate.HasValue ? startDate.Value.ToString("dd/MM/yyyy") : null;
            this.EndDate = endDate.HasValue ? endDate.Value.ToString("dd/MM/yyyy") : null;
        }

        public IDictionary<string, string> ToBody()
        {
            return new Dictionary<string, string>(Credentials.ToDictionary(x => x.Name, x => x.Value))
            {
                { "startDate", StartDate },
                { "endDate", EndDate }
            };
        }
    }
}
