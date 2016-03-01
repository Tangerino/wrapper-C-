using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Users;

namespace WimdioApiProxy.v2.Tests
{
    public abstract class BaseTests : IDisposable
    {
        protected readonly IWimdioApiClient Client;

        public BaseTests()
        {
            Client = Task.Run(() => GetAuthorizedClient()).Result;
        }

        public void Dispose()
        {
            Client.Logout();
        }

        protected static readonly Credentials Credentials = new Credentials
        {
            Email = "XpaMoBHuk",
            Password = "XpaMoBHuk"
        };

        internal static async Task<IWimdioApiClient> GetAuthorizedClient()
        {
            var client = new WimdioApiClient();
            await client.Login(Credentials);
            return client;
        }

        internal static async Task<User> CreateUser(IWimdioApiClient client, List<User> usersCreated)
        {
            var user = new NewUser
            {
                Password = "secure",
                FirstName = "FirstName",
                LastName = "LastName",
                Email = $"dummy+{Guid.NewGuid().ToString().Split('-').First()}@email.com",
                Permissions = Permission.Read | Permission.Update
            };

            var userCreated = await client.CreateUser(user);
            usersCreated?.Add(userCreated);

            return userCreated;
        }

        internal static async Task<Place> CreatePlace(IWimdioApiClient client, List<Place> placesCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var place = new NewPlace
            {
                Name = $"Name {random}",
                Description = $"Description {random}"
            };

            var placeCreated = await client.CreatePlace(place);
            placesCreated?.Add(placeCreated);

            return placeCreated;
        }

        internal static async Task<NormalizationFactor> CreateNormalizationFactor(IWimdioApiClient client, Place place, List<NormalizationFactor> normalizationFactorsCreated)
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

            var normalizationFactorCreated = await client.CreateNormalizationFactor(place.Id, normalizationFactor);
            normalizationFactorsCreated?.Add(normalizationFactorCreated);

            return normalizationFactorCreated;
        }

        internal static async Task<NormalizationFactorValue> CreateNormalizationFactorValue(IWimdioApiClient client, NormalizationFactor nf, Dictionary<Guid, NormalizationFactorValue> normalizationFactorValuesCreated)
        {
            var dateTime = DateTime.Now;
            var rnd = new Random();

            var normalizationFactorValue = new NormalizationFactorValue
            {
                Timestamp = dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond)),
                Value = rnd.Next(100000).ToString(),
            };

            await client.CreateNormalizationFactorValue(nf.Id, normalizationFactorValue);
            normalizationFactorValuesCreated?.Add(nf.Id, normalizationFactorValue);

            return normalizationFactorValue;
        }
    }
}
