using Newtonsoft.Json;

namespace WimdioApiProxy.v2.Helpers
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string input) where T : class
        {
            return JsonConvert.DeserializeObject<T>(input);
        }

        public string Serialize<T>(T input) where T : class
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}
