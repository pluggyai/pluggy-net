using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hermes.SDK.Model
{
    public class ExecutionParameters
    {
        [JsonProperty("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonProperty("credentials")]
        public List<ExecuteParameter> Credentials { get; set; }

        public ExecutionParameters()
        {
        }

        public ExecutionParameters(List<ExecuteParameter> credentials)
        {
            this.Credentials = credentials;
        }
    }
}
