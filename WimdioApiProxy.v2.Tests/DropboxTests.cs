using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WimdioApiProxy.v2.DataTransferObjects.DropBox;
using WimdioApiProxy.v2.DataTransferObjects.Devices;

namespace WimdioApiProxy.v2.Tests
{
    [TestClass()]
    public class DropboxTests : BaseTests
    {
        protected IDictionary<Guid, FileInfo> FilesCreated = new Dictionary<Guid, FileInfo>();
        protected List<Device> DevicesCreated = new List<Device>();

        public new void Dispose()
        {
            FilesCreated.ToList().ForEach(async f => await Client.DeleteFile(f.Key, f.Value.Id));
            DevicesCreated.ForEach(async device => await Client.DeleteDevice(device.Id));
            base.Dispose();
        }

        [TestMethod()]
        public void SendFileToDevice_Positive()
        {
            FileInfo actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await CreateFile(Client, device, FilesCreated);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
            actual.Id.Should().NotBe(Guid.Empty, "Id should not be empty");
            actual.Created.Should().BeBefore(DateTime.Now, "Created date time should be in the past");
        }

        [TestMethod()]
        public void ReadFilesInformation_Positive()
        {
            IEnumerable<FileInfo> actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                actual = await Client.ReadFilesInformation(device.Id, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1));
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull("Result list should not be NULL");
        }

        [TestMethod()]
        public void DeviceReadFileInfo_Positive()
        {
            DeviceFileInfo actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var file = new NewFile
                {
                    Url = new Uri("http://veryshorthistory.com/wp-content/uploads/2015/04/knights-templar.jpg"),
                    Action = FileAction.POST,
                    Type = "FIRMWARE_UPGRADE"
                };
                var device = await CreateDevice(Client, DevicesCreated);
                await Client.SendFileToDevice(device.Id, file);
                actual = await Client.DeviceReadFileInfo(device.DevKey);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }
    }
}
