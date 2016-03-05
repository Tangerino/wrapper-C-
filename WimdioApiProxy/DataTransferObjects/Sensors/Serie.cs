using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class Serie
    {
        [JsonProperty("remoteid")]
        public string RemoteId { get; set; }

        [JsonProperty("values")]
        public IDictionary<DateTime, decimal> Values { get; set; }
    }
}
