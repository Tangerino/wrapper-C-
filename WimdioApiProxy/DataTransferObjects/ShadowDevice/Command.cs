using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class Command
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("created")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime Created { get; set; }

        [JsonProperty("objectname")]
        public string ObjectName { get; set; }

        [JsonProperty("objectid")]
        public int ObjectId { get; set; }

        [JsonProperty("duedate")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DueDate { get; set; }
    }

}
