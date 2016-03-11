﻿using System;
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
            var expected = new NewFile
            {
                Url = new Uri("http://veryshorthistory.com/wp-content/uploads/2015/04/knights-templar.jpg"),
                Action = FileAction.POST,
                Type = FileType.GENERIC
            };
            FileInfo actual = null;
            Func<Task> asyncFunction = async () =>
            {
                var device = await CreateDevice(Client, DevicesCreated);
                var file = await Client.SendFileToDevice(device.Id, expected);
                actual = (await Client.ReadFilesInformation(device.Id, DateTime.Now.AddHours(-1), DateTime.Now.AddHours(1)))?.FirstOrDefault(x => x.Url == expected.Url);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
            actual.Should().NotBeNull();
            actual.Action.Should().Be(expected.Action);
            actual.Type.Should().Be(expected.Type);
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
                    Type = FileType.FIRMWARE_UPGRADE
                };
                var device = await CreateDevice(Client, DevicesCreated);
                await Client.SendFileToDevice(device.Id, file);
                actual = await Client.DeviceReadFileInfo(device.DevKey);
            };
            asyncFunction.ShouldNotThrow("Method should not throw");
        }

        [TestMethod()]
        public void DeviceAcknowledgeFile_Positive()
        {
            throw new NotImplementedException("Can not implement method while read files returns an empty array");
        }

        [TestMethod()]
        public void DeleteFile_Positive()
        {
            throw new NotImplementedException("Can not implement method while read files returns an empty array");
        }
    }
}
