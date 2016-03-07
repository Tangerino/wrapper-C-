using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class UsersTests : BaseTests
    {
        protected List<User> UsersCreated = new List<User>();

        public new void Dispose()
        {
            UsersCreated.ForEach(async u => await Client.DeletePlace(u.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadUses_Positive()
        {
            IEnumerable<User> actual = null;

            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadUsers();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Users list should not be NULL");
        }

        [TestMethod()]
        public void CreateUser_Positive()
        {
            User actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreateUser(Client, UsersCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
        }

        [TestMethod()]
        public void ReadUser_Positive()
        {
            User expected = null;
            User actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreateUser(Client, UsersCreated);
                actual = await Client.ReadUser(expected.Id);
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
            User expected = null;
            User actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreateUser(Client, UsersCreated);
                var update = new UpdateUser(expected)
                {
                    FirstName = expected.FirstName + "Updated",
                };
                actual = await Client.UpdateUser(expected.Id, update);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
            actual.FirstName.Should().NotBe(expected.FirstName, "Unexpected user firstname");
            actual.LastName.Should().Be(expected.LastName, "Unexpected user lastname");
        }

        [TestMethod()]
        public void ChangePermissions_Positive()
        {
            Permission expected = Permission.Create | Permission.Update | Permission.Read;
            User actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var user = await CreateUser(Client, UsersCreated);

                actual = await Client.ChangePermissions(user.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("User should not be NULL");
            actual.Permissions.Should().Be(expected, "Unexpected user permissions");
        }

        [TestMethod()]
        public void DeleteUser_Positive()
        {
            Func<Task> asyncFunction = async () =>
            {
                var user = await CreateUser(Client, UsersCreated);
                await Client.DeleteUser(user.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
