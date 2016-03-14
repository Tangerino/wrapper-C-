using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Devices;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class DevicesTests : BaseTests
    {
        [TestMethod()]
        public void ReadDevices_Positive()
        {
            IEnumerable<Device> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await Client.ReadDevices();
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void Device_CRUD_Positive()
        {
            Device device = null;
            Func<Task> asyncFunction = async () => device = await CreateDevice(Client);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();

            asyncFunction = async () => device = await Client.ReadDevice(device.Id);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();

            var update = new UpdateDevice(device)
            {
                Name = device.Name + " Updated",
                Description = device.Description + " Updated",
            };
            asyncFunction = async () => device = await Client.UpdateDevice(device.Id, update);
            asyncFunction.ShouldNotThrow();
            device.Should().NotBeNull();
            device.Name.Should().NotBe(update.Name);
            device.Description.Should().NotBe(update.Description);
            device.Mac.Should().Be(update.Mac);

            asyncFunction = async () => await Client.DeleteDevice(device.Id);
            asyncFunction.ShouldNotThrow();

            asyncFunction = async () => device = await Client.ReadDevice(device.Id);
            asyncFunction.ShouldNotThrow();
            device.Should().BeNull();
        }
    }
}
