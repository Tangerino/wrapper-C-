
namespace WimdioApiProxy.v2.Rest
{
    internal class ApiRequestClient : AuthenticationClient
    {
        public ApiRequestClient(string baseUrl, string apiKey, int? requestTimeoutInSeconds = null)  : base(baseUrl, requestTimeoutInSeconds)
        {
            if (apiKey != null)
            {
                CustomHeaders.Add("apikey", apiKey);
            }
        }
    }
}
