using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2
{
    public partial interface IWimdioApiClient
    {
        Task Login(Credentials credentials);
        Task Logout();
        Task<string> ChangePassword(Credentials credentials);

        Task<string> ResetAccount(Account account);

        Task CreatePocket(string pocketName, object pocket);
        Task DeletePocket(string pocketName);
        Task<dynamic> ReadPocket(string pocketName);

        Task<IEnumerable<User>> ReadUsers();
        Task<User> CreateUser(NewUser user);
        Task<User> ReadUser(Guid userId);
        Task<User> UpdateUser(Guid userId, UpdateUser user);
        Task DeleteUser(Guid userId);
        Task<User> ChangePermissions(Guid userId, Permission permissions);
    }
}
