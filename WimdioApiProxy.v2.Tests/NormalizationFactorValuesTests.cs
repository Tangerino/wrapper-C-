using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class NormalizationFactorValuesTests : BaseTests
    {
        protected List<Place> PlacesCreated = new List<Place>();
        protected List<NormalizationFactor> NormalizationFactorsCreated = new List<NormalizationFactor>();
        protected Dictionary<Guid, NormalizationFactorValue> NormalizationFactorValuesCreated = new Dictionary<Guid, NormalizationFactorValue>();

        public new void Dispose()
        {
            NormalizationFactorValuesCreated.ToList().ForEach(v => Client.DeleteNormalizationFactorValue(v.Key, v.Value.Timestamp));
            NormalizationFactorsCreated.ForEach(nf => Client.DeleteNormalizationFactor(nf.Id));
            PlacesCreated.ForEach(async p => await Client.DeletePlace(p.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadNormalizationFactorValues_Positive()
        {
            IEnumerable<NormalizationFactorValue> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var nf = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
                actual = await Client.ReadNormalizationFactorValues(nf.Id);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Should().BeEmpty("Actual value should be empty");
        }

        [TestMethod()]
        public void CreateNormalizationFactorValue_Positive()
        {
            NormalizationFactorValue expected = null;
            NormalizationFactorValue actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var nf = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
                expected = await CreateNormalizationFactorValue(Client, nf, NormalizationFactorValuesCreated);
                actual = (await Client.ReadNormalizationFactorValues(nf.Id))?.FirstOrDefault();
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Timestamp.Should().Be(expected.Timestamp, "Unexpected actual timestamp");
            actual.Value.Should().Be(expected.Value, "Unexpected actual value");
        }

        [TestMethod()]
        public void UpdateNormalizationFactorValue_Positive()
        {
            NormalizationFactorValue expected = null;
            NormalizationFactorValue actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var nf = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
                expected = await CreateNormalizationFactorValue(Client, nf, NormalizationFactorValuesCreated);
                var update = new UpdateNormalizationFactorValue(expected)
                {
                    Value = expected.Value + "12345"
                };
                actual = await Client.UpdateNormalizationFactorValue(nf.Id, update);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Timestamp.Should().Be(expected.Timestamp, "Unexpected timestamp");
            actual.Value.Should().NotBe(expected.Value, "Unexpected value");
        }

        [TestMethod()]
        public void DeleteNormalizationFactorValue_Positive()
        {
            NormalizationFactorValue actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var nf = await CreateNormalizationFactor(Client, place, NormalizationFactorsCreated);
                var value = await CreateNormalizationFactorValue(Client, nf, null);

                await Client.DeleteNormalizationFactorValue(nf.Id, value.Timestamp);
                actual = (await Client.ReadNormalizationFactorValues(nf.Id))?.FirstOrDefault(x => x.Equals(value.Timestamp));
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNull("Actual value should be NULL");
        }
    }
}
