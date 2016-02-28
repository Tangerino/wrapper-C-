using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class WimdioApiClientTests
    {
        private readonly Credentials _credentials = new Credentials
        {
            Email = "XpaMoBHuk",
            Password = "XpaMoBHuk"
        };

        private async Task<IWimdioApiClient> GetAuthorizedClient()
        {
            var client = new WimdioApiClient();

            var error = await client.Login(_credentials);

            if (!string.IsNullOrEmpty(error))
                throw new Exception(error);

            return client;
        }

        [TestMethod()]
        public void Login_Positive()
        {
            Func<Task> asyncFunction = async () =>
            {
                await GetAuthorizedClient();
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
        }

        [TestMethod()]
        public void Logout_Positive()
        {
            string actual = null;

            Func<Task> asyncFunction = async () =>
            {
                var client = await GetAuthorizedClient();
                actual = await client.Logout();
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNullOrEmpty("Error was not expected");
        }

        [TestMethod()]
        [Ignore()]
        public void ChangePassword_Positive()
        {
            string actual = null;
            string expected = "An e-mail was sent. Please follow instructions";

            Func<Task> asyncFunction = async () =>
            {
                var client = await GetAuthorizedClient();
                actual = await client.ChangePassword(_credentials);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().Be(expected, "Method should return specific message");
        }

        [TestMethod()]
        public void ChangePermissions_Positive()
        {
            const int userId = 12345;
            var permissions = new Permissions { Read = true };
            string actual = null;

            Func<Task> asyncFunction = async () =>
            {
                var client = await GetAuthorizedClient();
                actual = await client.ChangePermissions(userId, permissions);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNullOrEmpty("Method should not return error");
        }

        [TestMethod()]
        [Ignore()]
        public void ResetAccount_Positive()
        {
            string actual = null;
            string expected = "An e-mail was sent. Please follow instructions";

            Func<Task> asyncFunction = async () =>
            {
                var client = await GetAuthorizedClient();
                actual = await client.ResetAccount(new Account { Email = _credentials.Email });
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().Be(expected, "Method should return specific message");
        }

        [TestMethod()]
        public void Pocket_Positive()
        {
            var pocketName = "TestPocketName";
            IWimdioApiClient client = null;
            string createResult = null;
            string deleteResult = null;
            var expected = new { favorites1 = "dashboards", favorites2 = "alarms" };
            object actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                createResult = await client.CreatePocket(pocketName, expected);

                actual = await client.ReadPocket(pocketName);

                deleteResult = await client.DeletePocket(pocketName);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            createResult.Should().BeNullOrEmpty("Method should not return error");
            actual.Should().NotBeNull("Pocket content expected");
            deleteResult.Should().BeNullOrEmpty("Method should not return error");
        }
    }
}