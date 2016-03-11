using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class PlacesTests : BaseTests
    {
        protected List<Place> PlacesCreated = new List<Place>();

        public new void Dispose()
        {
            PlacesCreated.ForEach(async p => await Client.DeletePlace(p.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadPlaces_Positive()
        {
            IEnumerable<Place> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadPlaces();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreatePlace_Positive()
        {
            Place actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreatePlace(Client, PlacesCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadPlace_Positive()
        {
            Place expected = null;
            Place actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreatePlace(Client, PlacesCreated);
                actual = await Client.ReadPlace(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void UpdatePlace_Positive()
        {
            Place expected = null;
            Place actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreatePlace(Client, PlacesCreated);
                var updated = new UpdatePlace(expected)
                {
                    Name = expected.Name + "Updated",
                };
                actual = await Client.UpdatePlace(expected.Id, updated);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().NotBe(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void LinkUnlinkPlace_Positive()
        {
            Place actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreatePlace(Client, PlacesCreated);
                var user = await CreateUser(Client, null);

                await Client.LinkPlace(actual.Id, user.Id);
                await Client.UnlinkPlace(actual.Id, user.Id);

                await Client.DeleteUser(user.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void DeletePlace_Positive()
        {
            Place actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreatePlace(Client, PlacesCreated);
                await Client.DeletePlace(actual.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
