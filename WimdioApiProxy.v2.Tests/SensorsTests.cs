using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Sensors;
using WimdioApiProxy.v2.DataTransferObjects.Devices;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class SensorsTests : BaseTests
    {
        protected IDictionary<string, Sensor> SensorsCreated = new Dictionary<string, Sensor>();
        protected List<Device> DevicesCreated = new List<Device>();

        public new void Dispose()
        {
            SensorsCreated.ToList().ForEach(async s => await Client.DeleteSensor(s.Key, s.Value.RemoteId));
            DevicesCreated.ForEach(async d => await Client.DeleteThing(d.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ReadSensors_Positive()
        {
            IEnumerable<Sensor> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await Client.ReadSensors(device.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeEmpty("Actual value should be NULL");
        }

        [TestMethod()]
        public void CreateSensor_Positive()
        {
            Sensor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await CreateSensor(Client, device, SensorsCreated);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void UpdateSensor_Positive()
        {
            Sensor expected = null;
            Sensor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                expected = await CreateSensor(Client, device, SensorsCreated);
                var update = new UpdateSensor(expected)
                {
                    Description = expected.Description + "Updated",
                    Tseoi = 1,
                };
                actual = await Client.UpdateSensor(device.DevKey, expected.RemoteId, update);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Actual name should not be updated");
            actual.Description.Should().NotBe(expected.Description, "Actual description should be updated");
            actual.Unit.Should().Be(expected.Unit, "Actual unit should not be updated");
            actual.Tseoi.Should().NotBe(expected.Tseoi, "Actual TSEOI should be updated");
        }
    }
}
