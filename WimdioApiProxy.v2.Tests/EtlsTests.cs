using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class EtlsTests : BaseTests
    {
        protected List<Etl> EtlsCreated = new List<Etl>();
        protected List<Place> PlacesCreated = new List<Place>();

        public new void Dispose()
        {
            PlacesCreated.ForEach(async p => await Client.DeletePlace(p.Id));
            EtlsCreated.ForEach(async t => await Client.DeleteEtl(t.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadEtls_Positive()
        {
            IEnumerable<Etl> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadEtls();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreateEtl_Positive()
        {
            Etl actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await CreateEtl(Client, place, EtlsCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadEtl_Positive()
        {
            Etl expected = null;
            Etl actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                expected = await CreateEtl(Client, place, EtlsCreated);
                actual = await Client.ReadEtl(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
        }

        [TestMethod()]
        public void UpdateEtl_Positive()
        {
            NewEtl expected = null;
            Etl actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var etl = await CreateEtl(Client, place, EtlsCreated);
                expected = new NewEtl
                {
                    Name = etl.Name + "Updated",
                };
                actual = await Client.UpdateEtl(place.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
        }

        [TestMethod()]
        public void DeleteEtl_Positive()
        {
            Etl actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await CreateEtl(Client, place, EtlsCreated);

                await Client.DeleteEtl(actual.Id);
                actual = await Client.ReadEtl(actual.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNull("Actual value should be NULL");
        }
    }
}
