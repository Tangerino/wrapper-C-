using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class ThingsTests : BaseTests
    {
        protected List<Thing> ThingsCreated = new List<Thing>();
        protected List<Place> PlacesCreated = new List<Place>();

        public new void Dispose()
        {
            PlacesCreated.ForEach(async p => await Client.DeletePlace(p.Id));
            ThingsCreated.ForEach(async t => await Client.DeleteThing(t.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadThings_Positive()
        {
            IEnumerable<Thing> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await Client.ReadThings(place.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreateThing_Positive()
        {
            Thing actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await CreateThing(Client, place, ThingsCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadThing_Positive()
        {
            Thing expected = null;
            Thing actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                expected = await CreateThing(Client, place, ThingsCreated);
                actual = await Client.ReadThing(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void UpdateThing_Positive()
        {
            NewThing expected = null;
            Thing actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var thing = await CreateThing(Client, place, ThingsCreated);
                expected = new NewThing
                {
                    Name = thing.Name + "Updated",
                    Description = thing.Description + "Updated",
                };
                actual = await Client.UpdateThing(place.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void DeleteThing_Positive()
        {
            Thing actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await CreateThing(Client, place, ThingsCreated);

                await Client.DeleteThing(actual.Id);
                actual = await Client.ReadThing(actual.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNull("Actual value should be NULL");
        }
    }
}
