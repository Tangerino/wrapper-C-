using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Accounts
{
    public class LoginResponse
    {
        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
    }
}
