using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Sensors
{
    public class Sensor : SensorUpdate
    {
        [JsonProperty("remoteid")]
        public string RemoteId { get; set; }
    }
}
