using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WimdioApiProxy.v2.DataTransferObjects.ShadowDevice
{
    public class NewCommand : CommandBase
    {
        [JsonProperty("action")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CommandAction Action { get; set; }

        [JsonProperty("objectcontent")]
        public ObjectContent ObjectContent { get; set; }
    }

    public class ObjectContent
    {
        [JsonProperty("publishinginterval")]
        public int PublishingInterval { get; set; }
    }

    public enum CommandAction
    {
        UPDATE
    }
}
