using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public class UpdateUser
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }

        [JsonProperty("lastname")]
        public string LastName { get; set; }
    }
}
