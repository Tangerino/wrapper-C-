using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.Devices;
using WimdioApiProxy.v2.DataTransferObjects.ShadowDevice;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class ShadowDeviceTests : BaseTests
    {
        [TestMethod()]
        public void ShadowCommands_CarlosTestCase_Positive()
        {
            // Please do not delete it
            // It is a real device sending data to the WIMD and has a lot of objetcs in it
            var testDeviceId = Guid.Parse("01344f33-edb8-11e5-8a0f-04017fd5d401");
            Device device = null;
            Func<Task> asyncFunction = async () => device = await Client.ReadDevice(testDeviceId);
            asyncFunction.ShouldNotThrow($"ReadDevice({testDeviceId}) shoud not throw");
            device.Should().NotBeNull($"test device ID = {testDeviceId} should exist");
            device.Id.Should().Be(testDeviceId);

            // Gateway requests commands
            var limit = 10;
            IEnumerable<Command> commands = null;
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {limit}) shoud not throw");
            commands.Should().NotBeNull("ReadDeviceCommands should return empty array");
            commands.Should().BeEmpty("ReadDeviceCommands should return empty array");

            // Gateway send some configuration
            var settings = CarlosTestSettings();
            asyncFunction = async () => await Client.SendDeviceSettings(device.DevKey, settings);
            asyncFunction.ShouldNotThrow($"SendDeviceSettings({device.DevKey}, settings) shoud not throw");

            // NORTH API read devices
            testDeviceId = Guid.Parse("01344f33-edb8-11e5-8a0f-04017fd5d401");
            IEnumerable <Device> devices = null;
            asyncFunction = async () => devices = await Client.ReadDevices();
            asyncFunction.ShouldNotThrow($"ReadDevices() shoud not throw");
            devices.Should().NotBeNullOrEmpty("ReadDevices() should return non empty list of devices");
            devices.Any(x => x.Id == testDeviceId).Should().BeTrue($"ReadDevices() result should contain a device ID = {testDeviceId}");
            device = devices.First(x => x.Id == testDeviceId);

            // read the available objects from device
            var testObjectName = "cloudservice";
            IEnumerable<ShadowObjectName> shadowObjectNames = null;
            asyncFunction = async () => shadowObjectNames = await Client.ReadDeviceObjects(device.Id);
            asyncFunction.ShouldNotThrow($"ReadDeviceObjects({device.Id}) should not throw");
            shadowObjectNames.Should().NotBeNullOrEmpty($"ReadDeviceObjects({device.Id}) should return not empty list");
            shadowObjectNames.Any(x => x.ObjectName.Equals(testObjectName, StringComparison.InvariantCultureIgnoreCase)).Should().BeTrue($"ReadDeviceObjects({device.Id}) result should contain an object '{testObjectName}'");
            var testObject = shadowObjectNames.First(x => x.ObjectName.Equals(testObjectName, StringComparison.InvariantCultureIgnoreCase));

            // Read a particular object
            IEnumerable<ShadowObject> shadowObjects = null;
            asyncFunction = async () => shadowObjects = await Client.ReadDeviceObjects(device.Id, testObject.ObjectName, 1, 3);
            asyncFunction.ShouldNotThrow($"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') should not throw");
            shadowObjects.Should().NotBeNullOrEmpty($"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') should return non empty list");
            shadowObjects.Count().Should().Be(2, $"ReadDeviceObjects({device.Id}, '{testObject.ObjectName}') result list should contain 2 objects");
            var shadowObject = shadowObjects.First();

            // Create a command
            // As we do have the actual gateway settings, I'd like to change some values, in this case I'll update the parameter "cleanSession" to 1
            var newCommand = new NewCommand
            {
                ObjectName = testObject.ObjectName,
                ObjectId = shadowObject.Id,
                Action = CommandAction.UPDATE,
                ObjectContent = new ShadowObjectContent { CleanSession = true }
            };
            Command command = null;
            asyncFunction = async () => command = await Client.CreateDeviceCommand(device.Id, newCommand);
            asyncFunction.ShouldNotThrow($"CreateDeviceCommand({device.Id}, newCommand) should not throw");
            command.Should().NotBeNull($"CreateDeviceCommand({device.Id}, newCommand) should not return NULL");
            command.ObjectName.Should().Be(shadowObject.ObjectContent.Name);

            // Again, I'll update the "timeout" to 60 and add a comment to the new command. This will create a new command
            newCommand = new NewCommand
            {
                ObjectName = testObject.ObjectName,
                ObjectId = shadowObject.Id,
                Action = CommandAction.UPDATE,
                ObjectContent = new ShadowObjectContent { Timeout = 60 },
                Comment = "Increase timeout"
            };
            asyncFunction = async () => command = await Client.CreateDeviceCommand(device.Id, newCommand);
            asyncFunction.ShouldNotThrow($"CreateDeviceObject({device.Id}, newCommand) should not throw");
            command.Should().NotBeNull($"CreateDeviceObject({device.Id}, newCommand) should not return NULL");
            command.ObjectName.Should().Be(testObjectName);
            command.Comment.Should().Be(newCommand.Comment);

            // I'll read now the commands available to the remote device
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.Id, DateTime.MinValue, DateTime.MaxValue);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {DateTime.MinValue}, {DateTime.MaxValue}) shoud not throw");
            commands.Should().NotBeNullOrEmpty($"ReadDeviceCommands({device.DevKey}, {DateTime.MinValue}, {DateTime.MaxValue} should not return empty array");

            // Now the gateway read one command, execute it and acknowledge (device uses its DEVKEY)
            limit = 2;
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.DevKey, limit);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {limit}) shoud not throw");
            commands.Should().NotBeNull("ReadDeviceCommands should return empty array");
            commands.Should().NotBeEmpty("ReadDeviceCommands should return empty array");
            commands.Count().Should().Be(2);
            commands.All(x => x.ObjectId == shadowObject.Id).Should().BeTrue();

            // The gateway will execute the commands and send back the result to the server
            var commandStates = new List<CommandState>
            {
                new CommandState { Id = commands.First().Id, Status = 0 },
                new CommandState { Id = commands.Last().Id, Status = 13 },
            };
            asyncFunction = async () => await Client.AcknowledgeDeviceCommands(device.DevKey, commandStates);
            asyncFunction.ShouldNotThrow($"AcknowledgeDeviceCommands({device.DevKey}, commandStates) shoud not throw");

            // I'll now read back the commands I've sent. They are the same as before but the field acknowledge has a date and staus is also set
            asyncFunction = async () => commands = await Client.ReadDeviceCommands(device.Id, DateTime.MinValue, DateTime.MaxValue);
            asyncFunction.ShouldNotThrow($"ReadDeviceCommands({device.DevKey}, {DateTime.MinValue}, {DateTime.MaxValue}) shoud not throw");
            commands.Should().NotBeNullOrEmpty($"ReadDeviceCommands({device.DevKey}, {DateTime.MinValue}, {DateTime.MaxValue} should not return empty array");
            commands.All(x => x.Acknowledge.HasValue).Should().BeTrue();
            commands.All(x => x.Status.HasValue).Should().BeTrue();
        }

        [TestMethod()]
        public void ShadowObject_Deserialize_Positive()
        {
            var json = "[{\n\t\t\"id\":\t20,\n\t\t\"objectcontent\":\t\"{\\\"name\\\":\\\"wimd\\\",\\\"type\\\":18,\\\"enabled\\\":1,\\\"host\\\":1,\\\"publishinterval\\\":1440,\\\"tagposition\\\":31310287,\\\"lastrun\\\":\\\"2016-03-19T08:46:05\\\",\\\"nextrun\\\":\\\"9999-12-31T23:59:59.9999999\\\",\\\"status\\\":200,\\\"pause\\\":0,\\\"zipit\\\":0,\\\"cleanSession\\\":0,\\\"timeout\\\":30,\\\"activationcode\\\":\\\"d109a897-d26f-11e5-8d5d-04017fd5d401\\\",\\\"eventposition\\\":0,\\\"alarmposition\\\":0,\\\"configposition\\\":2}\"\n\t}, {\n\t\t\"id\":\t21,\n\t\t\"objectcontent\":\t\"{\\\"name\\\":\\\"WIMD.IO\\\",\\\"type\\\":18,\\\"enabled\\\":1,\\\"host\\\":11,\\\"publishinterval\\\":1,\\\"tagposition\\\":-1,\\\"lastrun\\\":\\\"2016-03-19 10:54:04\\\",\\\"nextrun\\\":\\\"2016-03-19 10:55:00\\\",\\\"status\\\":200,\\\"pause\\\":0,\\\"zipit\\\":0,\\\"apiKey\\\":null,\\\"cleanSession\\\":1,\\\"timeout\\\":30,\\\"activationcode\\\":\\\"013451c7-edb8-11e5-8a0f-04017fd5d401\\\",\\\"feedid\\\":null,\\\"mailto\\\":null,\\\"mailcc\\\":null,\\\"mailbcc\\\":null,\\\"eventposition\\\":-1,\\\"alarmposition\\\":-1,\\\"configposition\\\":-1}\"\n\t}]";
            var serializer = new JsonSerializer();

            IEnumerable<ShadowObject> expected = null;
            Action act = () => expected = serializer.Deserialize<ShadowObject[]>(json);
            act.ShouldNotThrow("Deserialize<ShadowObject[]>(expected) should not throw");
            expected.Should().NotBeNullOrEmpty("Deserialize<ShadowObject[]>(expected) should contain some objects");

            var actualStr = string.Empty;
            act = () => actualStr = serializer.Serialize(expected);
            act.ShouldNotThrow("Serialize(actual) should not throw");

            IEnumerable<ShadowObject> actual = null;
            act = () => actual = serializer.Deserialize<ShadowObject[]>(actualStr);
            act.ShouldNotThrow("Deserialize<ShadowObject[]>(expected) should not throw");
            actual.Should().NotBeNullOrEmpty("Deserialize<ShadowObject[]>(expected) should contain some objects");
        }
    }
}
