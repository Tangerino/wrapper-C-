using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Users
{
    public class User : BaseUser
    {
        [JsonProperty("id")]
        public Guid Id{ get; set; }
    }
}
