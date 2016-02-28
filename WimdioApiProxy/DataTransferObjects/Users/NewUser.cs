using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public class NewUser : BaseUser
    {
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
