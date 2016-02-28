using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;

namespace WimdioApiProxy.v2
{
    public partial interface IWimdioApiClient
    {
        Task<string> Login(Credentials credentials);
        Task<string> Logout();
        Task<string> ChangePassword(Credentials credentials);

        Task<string> ChangePermissions(int userId, Permissions permissions);
        Task<string> ResetAccount(Account account);

        Task<string> CreatePocket(string pocketName, object obj);
        Task<string> DeletePocket(string pocketName);
        Task<dynamic> ReadPocket(string pocketName);
    }
}
