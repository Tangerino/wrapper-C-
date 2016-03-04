using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects.Accounts;
using WimdioApiProxy.v2.DataTransferObjects.Etls;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.Formulas;
using WimdioApiProxy.v2.DataTransferObjects.NormalizationFactors;
using WimdioApiProxy.v2.DataTransferObjects.Places;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Users;
using WimdioApiProxy.v2.DataTransferObjects.DropBox;

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

            var created = await client.CreateUser(user);
            usersCreated?.Add(created);

            return created;
        }

        internal static async Task<Place> CreatePlace(IWimdioApiClient client, List<Place> placesCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var place = new NewPlace
            {
                Name = $"Name {random}",
                Description = $"Description {random}"
            };

            var created = await client.CreatePlace(place);
            placesCreated?.Add(created);

            return created;
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

            var created = await client.CreateNormalizationFactor(place.Id, normalizationFactor);
            normalizationFactorsCreated?.Add(created);

            return created;
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

        internal static async Task<Thing> CreateThing(IWimdioApiClient client, Place place, List<Thing> thingsCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var thing = new NewThing
            {
                Name = "Name " + random,
                Description = "Description " + random
            };

            var created = await client.CreateThing(place.Id, thing);
            thingsCreated?.Add(created);

            return created;
        }

        internal static async Task<Device> CreateDevice(IWimdioApiClient client, List<Device> devicesCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var device = new NewDevice
            {
                Name = $"Name {random}",
                Description = $"Description {random}",
                Mac = random
            };

            var created = await client.CreateDevice(device);
            devicesCreated?.Add(created);

            return created;
        }

        internal static async Task<Formula> CreateFormula(IWimdioApiClient client, List<Formula> formulasCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var formula = new NewFormula
            {
                Name = $"Name {random}",
                Code = $"ww = w * w\r\nvv = v * v\r\nr = math.sqrt(ww + vv)\r\nvm = w / r",
                Library = 0
            };

            var created = await client.CreateFormula(formula);
            created.Code = formula.Code;
            formulasCreated?.Add(created);

            return created;
        }

        internal static async Task<FileInfo> CreateFile(IWimdioApiClient client, Device device, IDictionary<Guid, FileInfo> FilesCreated)
        {
            var file = new NewFile
            {
                Url = new Uri("http://veryshorthistory.com/wp-content/uploads/2015/04/knights-templar.jpg"),
                Action = FileAction.POST,
                Type = "FIRMWARE_UPGRADE"
            };

            var created = await client.SendFileToDevice(device.Id, file);
            FilesCreated?.Add(device.Id, created);

            return created;
        }

        internal static async Task<Etl> CreateEtl(IWimdioApiClient client, Place place, List<Etl> etlsCreated)
        {
            var random = Guid.NewGuid().ToString().Split('-').First();

            var etl = new NewEtl
            {
                Name = $"Name {random}",
                Endpoint = new Uri("http://www.google.com"),
                Username = $"Username{random}",
                Password = $"{random}",
                Type = EtlType.InfluxDB,
                PlaceId = place.Id,
                DatabaseName = $"Database{random}",
                TableName = $"Table{random}",
            };

            var created = await client.CreateEtl(etl);
            etlsCreated?.Add(created);

            return created;
        }
    }
}
