using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.Tests
{
    public partial class WimdioApiClientTests
    {
        [TestMethod()]
        public void ReadUses_Positive()
        {
            IWimdioApiClient client = null;
            IEnumerable<User> actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await client.ReadUsers();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Users list should not be NULL");
        }


        [TestMethod()]
        public void CreateUser_Positive()
        {
            IWimdioApiClient client = null;
            User actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await CreateUser(client);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
        }

        [TestMethod()]
        public void ReadUser_Positive()
        {
            IWimdioApiClient client = null;
            User expected = null;
            User actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                expected = await CreateUser(client);
                actual = await client.ReadUser(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected user id");
            actual.Email.Should().Be(expected.Email, "Unexpected user email");
            actual.Permissions.Should().Be(expected.Permissions, "Unexpected user permissions");
        }

        [TestMethod()]
        public void UpdateUser_Positive()
        {
            IWimdioApiClient client = null;
            UpdateUser expected = null;
            User actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                var user = await CreateUser(client);

                expected = new UpdateUser
                {
                    FirstName = user.FirstName + "Updated",
                    LastName = user.LastName + "Updated",
                };

                actual = await client.UpdateUser(user.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
            actual.FirstName.Should().Be(expected.FirstName, "Unexpected user firstname");
            actual.LastName.Should().Be(expected.LastName, "Unexpected user lastname");
        }

        [TestMethod()]
        public void ChangePermissions_Positive()
        {
            IWimdioApiClient client = null;
            Permission expected = Permission.Create | Permission.Update | Permission.Read;
            User actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                var user = await CreateUser(client);

                actual = await client.ChangePermissions(user.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
            actual.Permissions.Should().Be(expected, "Unexpected user permissions");
        }

        [TestMethod()]
        public void Users_DeleteAll_Positive()
        {
            IWimdioApiClient client = null;
            IEnumerable<User> actualReadUsers = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();

                actualReadUsers = await client.ReadUsers();
                actualReadUsers.Select(u => u.Id).ToList().ForEach(async id => await client.DeleteUser(id));

                actualReadUsers = await client.ReadUsers();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actualReadUsers.Should().NotBeNull("Users list should not be NULL");
            actualReadUsers.Should().BeEmpty("Users list items were not expected");
        }

        private async Task<User> CreateUser(IWimdioApiClient client)
        {
            var user = new NewUser
            {
                Password = "secure",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = $"dummy+{Guid.NewGuid().ToString().Substring(0, 8)}@email.com",
                Permissions = Permission.Read | Permission.Update
            };

            return await client.CreateUser(user);
        }
    }
}
