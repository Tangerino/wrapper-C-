using Newtonsoft.Json;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public abstract class BaseUser : UpdateUser
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("permissions")]
        public Permission Permissions { get; set; }
    }
}
