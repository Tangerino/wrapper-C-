using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Devices
{
    public class Device : NewDevice
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
