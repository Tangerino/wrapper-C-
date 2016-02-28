using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Accounts
{
    public class Permissions
    {
        [JsonProperty("create")]
        public bool Create { get; set; }

        [JsonProperty("read")]
        public bool Read { get; set; }

        [JsonProperty("update")]
        public bool Update { get; set; }

        [JsonProperty("delete")]
        public bool Delete { get; set; }
    }
}
