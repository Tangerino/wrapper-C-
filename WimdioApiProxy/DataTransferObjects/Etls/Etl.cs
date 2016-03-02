using System;
using Newtonsoft.Json;

namespace WimdioApiProxy.v2.DataTransferObjects.Etls
{
    public class Etl : NewEtl
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
    }
}
