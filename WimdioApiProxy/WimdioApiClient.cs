using log4net;
using System;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.Rest;

namespace WimdioApiProxy.v2
{
    public class WimdioApiClient : IWimdioApiClient
    {
        private const string _baseUrl = "https://wimd.io/v2";
        private string _apiKey;

        private readonly ILog _log = LogManager.GetLogger(typeof(WimdioApiClient));

        public async Task<string> Login(Credentials credentials)
        {
            try
            {
                var client = new AuthenticationClient(_baseUrl);

                _apiKey = (await client.Post<LoginResponse>("account/login", credentials))?.ApiKey;

                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"Login(credentials.Email={credentials.Email}) failed", ex);

                return ex.Message;
            }
        }
        public async Task<string> Logout()
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/logout", new EmptyObject());

                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"Logout() failed", ex);

                return ex.Message;
            }
        }

        public async Task<string> ChangePassword(Credentials credentials)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/password", credentials);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePassword(credentials.Email={credentials.Email}) failed", ex);

                return ex.Message;
            }
        }
        public async Task<string> ChangePermissions(int userId, Permissions permissions)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>($"user/permissions/{userId}", permissions);

                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"ChangePermissions(userId={userId}, permissions) failed", ex);

                return ex.Message;
            }
        }
        public async Task<string> ResetAccount(Account account)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>("account/reset", account);

                return response.Status;
            }
            catch (Exception ex)
            {
                _log.Error($"ResetAccount(account.Email={account.Email}) failed", ex);

                return ex.Message;
            }
        }

        public async Task<string> CreatePocket(string pocketName, object obj)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Post<BasicResponse>($"account/pocket/{pocketName}", obj);

                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"CreatePocket(pocketName={pocketName}, obj='{obj}') failed", ex);

                return ex.Message;
            }
        }
        public async Task<string> DeletePocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Delete<BasicResponse>($"account/pocket/{pocketName}");

                return null;
            }
            catch (Exception ex)
            {
                _log.Error($"DeletePocket(pocketName={pocketName}) failed", ex);

                return ex.Message;
            }
        }
        public async Task<dynamic> ReadPocket(string pocketName)
        {
            try
            {
                var client = new ApiRequestClient(_baseUrl, _apiKey);

                var response = await client.Get<object>($"account/pocket/{pocketName}");

                return response;
            }
            catch (Exception ex)
            {
                _log.Error($"ReadPocket(pocketName={pocketName}) failed", ex);

                return ex.Message;
            }
        }
    }
}
