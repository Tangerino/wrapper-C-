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
        [TestMethod()]
        public void ShadowCommands_CRUD_Positive()
        {
            // create
            Device device = null;
            Command command = null;

            var rnd = new Random();
            var guid = Guid.NewGuid();
            var newCommand = new NewCommand
            {
                ObjectName = $"ObjectName{guid.ToString().Split('-').First()}",
                ObjectId = rnd.Next(99),
                Action = CommandAction.UPDATE,
                ObjectContent = new ObjectContent { PublishingInterval = rnd.Next(4) }
            };
            Func<Task> asyncFunction = async () => 
            {
                device = await CreateDevice(Client);
                command = await Client.CreateDeviceCommands(device.Id, newCommand);
            };
            asyncFunction.ShouldNotThrow();

            // read list
            IEnumerable<Command> commands = null;
            var limit = 3;
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow();
            commands.Should().NotBeNullOrEmpty();


            // aknowledge
            var commandStates = commands.Select(x => new CommandState { Id = x.Id }).ToList();
            commandStates.First().Status = 0;
            commandStates.Last().Error = 13;
            asyncFunction = async () => await Client.AcknowledgeDeviceCommands(device.DevKey, commandStates);
            asyncFunction.ShouldNotThrow();


            // delete
            commands.ToList().ForEach(c => 
            {
                asyncFunction = async () => await Client.DeleteDeviceCommands(device.Id, c.Id);
                asyncFunction.ShouldNotThrow();
            });


            // read list
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow();
            commands.Should().BeNullOrEmpty();
        }
    }
}
