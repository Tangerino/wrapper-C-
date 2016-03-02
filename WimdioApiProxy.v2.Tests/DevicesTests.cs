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
        protected List<Device> DevicesCreated = new List<Device>();

        public new void Dispose()
        {
            DevicesCreated.ForEach(async p => await Client.DeleteDevice(p.Id));
            base.Dispose();
        }

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
        public void CreateDevice_Positive()
        {
            Device actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreateDevice(Client, DevicesCreated);
            };

            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
        }

        [TestMethod()]
        public void ReadDevice_Positive()
        {
            Device expected = null;
            Device actual = null;
            Func<Task> asyncFunction = async () =>
            {
                expected = await CreateDevice(Client, DevicesCreated);
                actual = await Client.ReadDevice(expected.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Id.Should().Be(expected.Id, "Unexpected id");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void UpdateDevice_Positive()
        {
            NewDevice expected = null;
            Device actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                expected = new NewDevice
                {
                    Name = device.Name + "Updated",
                    Description = device.Description + "Updated",
                };
                actual = await Client.UpdateDevice(device.Id, expected);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Actual value should not be NULL");
            actual.Name.Should().Be(expected.Name, "Unexpected name");
            actual.Description.Should().Be(expected.Description, "Unexpected description");
        }

        [TestMethod()]
        public void DeleteDevice_Positive()
        {
            Device actual = null;
            Func<Task> asyncFunction = async () =>
            {
                actual = await CreateDevice(Client, DevicesCreated);
                await Client.DeleteDevice(actual.Id);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
