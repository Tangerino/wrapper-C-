using Newtonsoft.Json;

namespace WimdioApiProxy.v2.Helpers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings settings => new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto,
            DateFormatString = "yyyy-MM-ddTHH:mm:ss",
            NullValueHandling = NullValueHandling.Include,
        };

        public T Deserialize<T>(string input) where T : class
        {
            return JsonConvert.DeserializeObject<T>(input, settings);
        }

        public string Serialize<T>(T input) where T : class
        {
            return JsonConvert.SerializeObject(input, Formatting.Indented, settings);
        }
    }
}
