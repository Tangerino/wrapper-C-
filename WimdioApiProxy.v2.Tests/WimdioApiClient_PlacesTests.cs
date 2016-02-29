using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    public partial class WimdioApiClientTests
    {
        [TestMethod()]
        public void ReadPlaces_Positive()
        {
            IWimdioApiClient client = null;
            IEnumerable<Place> actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await client.ReadPlaces();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreatePlace_Positive()
        {
            IWimdioApiClient client = null;
            Place actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await CreatePlace(client);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadPlace_Positive()
        {
            IWimdioApiClient client = null;
            Place expected = null;
            Place actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                expected = await CreatePlace(client);
                actual = await client.ReadPlace(expected.Id);
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
            IWimdioApiClient client = null;
            NewPlace expected = null;
            Place actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                var place = await CreatePlace(client);

                expected = new NewPlace
                {
                    Name = place.Name + "Updated",
                    Description = place.Description + "Updated",
                };

                actual = await client.UpdatePlace(place.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void LinkUnlinkPlace_Positive()
        {
            IWimdioApiClient client = null;
            Place actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await CreatePlace(client);

                var user = await CreateUser(client);
                await client.LinkPlace(actual.Id, user.Id);

                await client.UnlinkPlace(actual.Id, user.Id);
                await client.DeleteUser(user.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }


        [TestMethod()]
        public void Places_DeleteAll_Positive()
        {
            IWimdioApiClient client = null;
            IEnumerable<Place> actualReadPlaces = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();

                actualReadPlaces = await client.ReadPlaces();
                actualReadPlaces.Select(p => p.Id).ToList().ForEach(async id => await client.DeletePlace(id));

                actualReadPlaces = await client.ReadPlaces();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actualReadPlaces.Should().NotBeNull("Result list should not be NULL");
            actualReadPlaces.Should().BeEmpty("Result list should be empty");
        }

        private async Task<Place> CreatePlace(IWimdioApiClient client)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var place = new NewPlace
            {
                Name = $"Name {random}",
                Description = $"Description {random}"
            };

            return await client.CreatePlace(place);
        }
    }
}
