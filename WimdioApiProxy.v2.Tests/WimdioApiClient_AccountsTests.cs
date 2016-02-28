using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public partial class WimdioApiClientTests
    {
        private readonly Credentials _credentials = new Credentials
        {
            Email = "XpaMoBHuk",
            Password = "XpaMoBHuk"
        };

        private async Task<IWimdioApiClient> GetAuthorizedClient()
        {
            var client = new WimdioApiClient();

            await client.Login(_credentials);

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
            Func<Task> asyncFunction = async () =>
            {
                var client = await GetAuthorizedClient();
                await client.Logout();
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
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
            var expected = new { favorites1 = "dashboards", favorites2 = "alarms" };
            object actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                await client.CreatePocket(pocketName, expected);

                actual = await client.ReadPocket(pocketName);

                await client.DeletePocket(pocketName);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Pocket content expected");
        }
    }
}