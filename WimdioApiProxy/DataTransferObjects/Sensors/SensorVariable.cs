using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class SensorVariable
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("variable")]
        public string Variable { get; set; }
    }
}
