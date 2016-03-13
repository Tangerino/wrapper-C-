using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Sensors;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.Helpers;
using WimdioApiProxy.v2.DataTransferObjects.Things;
using WimdioApiProxy.v2.DataTransferObjects.Places;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class SensorsTests : BaseTests
    {
        protected List<KeyValuePair<string, Sensor>> SensorsCreated = new List<KeyValuePair<string, Sensor>>();
        protected List<Thing> ThingsCreated = new List<Thing>();
        protected List<Place> PlacesCreated = new List<Place>();
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
        public void ReadSensor_Positive()
        {
            Sensor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await CreateSensor(Client, device, SensorsCreated);
                actual = await Client.ReadSensor(actual.Id);
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

        [TestMethod()]
        public void DeleteSensor_Positive()
        {
            Sensor actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await CreateSensor(Client, device, SensorsCreated);
                await Client.DeleteSensor(device.DevKey, actual.RemoteId);
                actual = (await Client.ReadSensors(device.Id))?.FirstOrDefault(x => x.RemoteId.Equals(actual.RemoteId));
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().BeNull("Actual should be deleted");
        }

        [TestMethod()]
        public void SerializeSensorData_Positive()
        {
            var expected = CreateSensorData(new[] { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() });
            SensorData actual = null;
            var serializer = new JsonSerializer();
            Action act = () => 
            {
                var json = serializer.Serialize(expected);
                actual = serializer.Deserialize<SensorData>(json);
            };
            act.ShouldNotThrow();
            actual.Should().NotBeNull();
            actual.Series?.FirstOrDefault()?.Serie?.RemoteId.Should().Be(expected.Series.FirstOrDefault().Serie.RemoteId);
            actual.Series?.FirstOrDefault()?.Serie?.Values?.FirstOrDefault()?[0].ToString().Should().Be(expected.Series.FirstOrDefault().Serie.Values.FirstOrDefault()[0].ToString());
            actual.Series?.FirstOrDefault()?.Serie?.Values?.FirstOrDefault()?[1].ToString().Should().Be(expected.Series.FirstOrDefault().Serie.Values.FirstOrDefault()[1].ToString());
        }

        [TestMethod()]
        public void SensorAddData_Positive()
        {
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor1 = await CreateSensor(Client, device, SensorsCreated);
                var sensor2 = await CreateSensor(Client, device, SensorsCreated);
                var data = CreateSensorData(new[] { sensor1.RemoteId, sensor2.RemoteId });
                await Client.SensorAddData(device.DevKey, data.Series);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }

        [TestMethod()]
        public void ReadSensorRule_Positive()
        {
            Rule actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor = await CreateSensor(Client, device, SensorsCreated);
                actual = await Client.ReadSensorRule(sensor.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void UpdateSensorRule_Positive()
        {
            UpdateRule expected = null;
            Rule actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor = await CreateSensor(Client, device, SensorsCreated);
                var rule = await Client.ReadSensorRule(sensor.Id);
                expected = new UpdateRule(rule);
                expected.Description = "UpdateSensorRule";
                actual = await Client.UpdateSensorRule(sensor.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void LinkUnlinkSensor_Positive()
        {
            IEnumerable<Sensor> actual = null;
            Sensor expected = null;
            Func<Task> asyncFunction = async () =>
            {
                var place = await CreatePlace(Client, PlacesCreated);
                var thing = await CreateThing(Client, place, ThingsCreated);
                var device = await CreateDevice(Client, DevicesCreated);
                expected = await CreateSensor(Client, device, SensorsCreated);

                await Client.LinkSensor(thing.Id, expected.Id);
                actual = await Client.ListSensors(thing.Id);
                await Client.UnlinkSensor(thing.Id, expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNullOrEmpty();
            actual.First().Id.Should().Be(expected.Id);
        }

        [TestMethod()]
        public void ReadVirtualSensorVariables_Positive()
        {
            IEnumerable<SensorVariable> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor = await CreateSensor(Client, device, SensorsCreated);
                actual = await Client.ReadVirtualSensorVariables(sensor.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull();
        }

        [TestMethod()]
        public void AddVirtualSensorVariables_Positive()
        {
            List<SensorVariable> expected = null; ;
            IEnumerable<SensorVariable> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor = await CreateSensor(Client, device, SensorsCreated);
                expected = new List<SensorVariable> { new SensorVariable { Id = sensor.Id.ToString(), Variable = "Dummy" } };
                await Client.AddVirtualSensorVariables(sensor.Id, expected);
                actual = await Client.ReadVirtualSensorVariables(sensor.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNullOrEmpty();
            actual.First().Id.Should().Be(expected.First().Id);
            actual.First().Variable.Should().Be(expected.First().Variable);
        }

        [TestMethod()]
        public void DeleteVirtualSensorVariables_Positive()
        {
            List<SensorVariable> expected = null; ;
            IEnumerable<SensorVariable> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var sensor = await CreateSensor(Client, device, SensorsCreated);
                expected = new List<SensorVariable> { new SensorVariable { Id = sensor.Id.ToString(), Variable = "Dummy" } };
                await Client.AddVirtualSensorVariables(sensor.Id, expected);
                actual = await Client.ReadVirtualSensorVariables(sensor.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull();
            actual.First().Id.Should().Be(expected.First().Id);
            actual.First().Variable.Should().Be(expected.First().Variable);
        }

        [TestMethod()]
        public void ReadVirtualSensors_Positive()
        {
            IEnumerable<Sensor> actual = null;
            Sensor expected = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                expected = await CreateSensor(Client, device, SensorsCreated);
                var virtualVariables = new List<SensorVariable> { new SensorVariable { Id = expected.Id.ToString(), Variable = "Dummy" } };
                await Client.AddVirtualSensorVariables(expected.Id, virtualVariables);
                actual = await Client.ReadVirtualSensors(device.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNullOrEmpty();
        }
    }
}
