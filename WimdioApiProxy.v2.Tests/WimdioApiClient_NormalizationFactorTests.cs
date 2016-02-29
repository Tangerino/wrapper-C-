using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;

namespace WimdioApiProxy.v2.Tests
{
    public partial class WimdioApiClientTests
    {
        [TestMethod()]
        public void ReadNormalizationFactors_Positive()
        {
            IWimdioApiClient client = null;
            IEnumerable<NormalizationFactor> actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();

                var place = await CreatePlace(client);

                actual = await client.ReadNormalizationFactors(place.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void CreateNormalizationFactor_Positive()
        {
            IWimdioApiClient client = null;
            NewNormalizationFactor actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                actual = await CreateNormalizationFactor(client);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadNormalizationFactor_Positive()
        {
            IWimdioApiClient client = null;
            NormalizationFactor expected = null;
            NormalizationFactor actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                expected = await CreateNormalizationFactor(client);
                actual = await client.ReadNormalizationFactor(expected.Id);
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
            IWimdioApiClient client = null;
            NewNormalizationFactor expected = null;
            NormalizationFactor actual = null;

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();
                var normalizationFactor = await CreateNormalizationFactor(client);

                expected = new NewNormalizationFactor
                {
                    Name = normalizationFactor.Name + "Updated",
                    Description = normalizationFactor.Description + "Updated",
                    Unit = normalizationFactor.Unit + "Updated"
                };

                actual = await client.UpdateNormalizationFactor(normalizationFactor.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void NormalizationFactor_DeleteAll_Positive()
        {
            IWimdioApiClient client = null;
            var actualReadNormalizationFactors = new List<NormalizationFactor>();

            Func<Task> asyncFunction = async () =>
            {
                client = await GetAuthorizedClient();

                (await client.ReadPlaces()).ToList().ForEach(async place => 
                {
                    (await client.ReadNormalizationFactors(place.Id)).ToList().ForEach(async nf => 
                    {
                        await client.DeleteNormalizationFactor(nf.Id);

                        actualReadNormalizationFactors.AddRange(await client.ReadNormalizationFactors(nf.Id));
                    });
                });
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actualReadNormalizationFactors.Should().BeEmpty("Result list should be empty");
        }

        private async Task<NormalizationFactor> CreateNormalizationFactor(IWimdioApiClient client)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var normalizationFactor = new NewNormalizationFactor
            {
                Name = $"Name {random}",
                Description = $"Description {random}",
                Aggregation = AggregationType.Average,
                Operation = Operation.Divide,
                Unit = $"Unit {random}"
            };

            var place = await CreatePlace(client);

            return await client.CreateNormalizationFactor(place.Id, normalizationFactor);
        }
    }
}
