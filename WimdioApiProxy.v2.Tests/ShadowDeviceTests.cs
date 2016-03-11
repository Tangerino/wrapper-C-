using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.ShadowDevice;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class ShadowDeviceTests : BaseTests
    {
        protected List<Device> DevicesCreated = new List<Device>();

        public new void Dispose()
        {
            DevicesCreated.ForEach(async d => await Client.DeleteDevice(d.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void ShadowCommands_Positive()
        {
            Device device = null;
            Func<Task> f = async () => device = await CreateDevice(Client, DevicesCreated);
            f.ShouldNotThrow();
            device.Should().NotBeNull();


            var rnd = new Random();
            var guid = Guid.NewGuid();
            IEnumerable<Command> commands = new List<Command>
            {
                new Command
                {
                    Id = guid,
                    Created = DateTime.Now,
                    ObjectName = $"ObjectName{guid.ToString().Split('-').First()}",
                    ObjectId = rnd.Next(99),
                    DueDate = DateTime.Now.AddDays(7)
                },
                new Command
                {
                    Id = guid,
                    Created = DateTime.Now,
                    ObjectName = $"ObjectName{guid.ToString().Split('-').First()}",
                    ObjectId = rnd.Next(99),
                    DueDate = DateTime.Now.AddDays(7)
                },
            };
            f = async () => await Client.CreateDeviceCommands(device.Id, commands);
            f.ShouldNotThrow();


            var commandStates = commands.Select(x => new CommandState { Id = x.Id }).ToList();
            commandStates.First().Status = 0;
            commandStates.Last().Error = 13;
            f = async () => await Client.AcknowledgeDeviceCommands(device.DevKey, commandStates);
            f.ShouldNotThrow();


            var limit = 3;
            f = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            f.ShouldNotThrow();
            commands.Should().NotBeNull();
            commands.Should().NotBeEmpty();


            commands.ToList().ForEach(c => 
            {
                f = async () => await Client.DeleteDeviceCommands(device.Id, c.Id);
                f.ShouldNotThrow();
            });


            f = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            f.ShouldNotThrow();
            commands.Should().NotBeNull();
            commands.Should().BeEmpty();
        }
    }
}
