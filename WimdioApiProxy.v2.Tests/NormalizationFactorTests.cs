using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class NormalizationFactorTests : BaseTests
    {
        protected List<Place> PlacesCreated = new List<Place>();
        protected List<NormalizationFactor> NormalizationFactorsCreated = new List<NormalizationFactor>();

        public new void Dispose()
        {
            NormalizationFactorsCreated.ForEach(nf => Client.DeleteNormalizationFactor(nf.Id));
            PlacesCreated.ForEach(async p => await Client.DeletePlace(p.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadNormalizationFactors_Positive()
        {
            IEnumerable<NewNormalizationFactor> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await Client.ReadNormalizationFactors(place.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreateNormalizationFactor_Positive()
        {
            NewNormalizationFactor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                actual = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadNormalizationFactor_Positive()
        {
            NormalizationFactor expected = null;
            NormalizationFactor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                expected = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
                actual = await Client.ReadNormalizationFactor(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void UpdateNormalizationFactor_Positive()
        {
            NewNormalizationFactor expected = null;
            NormalizationFactor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var normalizationFactor = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);

                expected = new NewNormalizationFactor
                {
                    Name = normalizationFactor.Name + "Updated",
                    Description = normalizationFactor.Description + "Updated",
                    Unit = normalizationFactor.Unit + "Updated"
                };

                actual = await Client.UpdateNormalizationFactor(normalizationFactor.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void DeleteNormalizationFactor_Positive()
        {
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var normalizationFactor = await CreateNormalizationFactor(Client, place, null);
                await Client.DeleteNormalizationFactor(normalizationFactor.Id);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
