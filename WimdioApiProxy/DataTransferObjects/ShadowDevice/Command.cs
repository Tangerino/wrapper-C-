using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class Command : CommandBase
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("duedate")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DueDate { get; set; }
    }
}
