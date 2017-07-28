using BlobService.Core.Tests.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BlobService.Core.Tests
{
    internal static class TestData
    {
        static TestData()
        {
            BlobMetaSeed.Add(new BlobMetaTest()
            {
                Id = "1",
                ContainerId = "1",
                MimeType = "application/pdf",
                StorageSubject = Sotragesubject,
                OrigFileName = "test.pdf",
                SizeInBytes = 500
            });

            BlobMetaSeed.Add(new BlobMetaTest()
            {
                Id = "2",
                ContainerId = "1",
                MimeType = "application/zip",
                StorageSubject = "SS_2",
                OrigFileName = "test.zip",
                SizeInBytes = 400
            });

            ContainerMetaSeed.Add(new ContainerMetaTest()
            {
                Id = "1",
                Name = "Test"
            });

            ContainerMetaSeed.First().Blobs.Add(BlobMetaSeed.ElementAt(0));
            ContainerMetaSeed.First().Blobs.Add(BlobMetaSeed.ElementAt(1));

            BlobMetaSeed.ElementAt(0).Container = ContainerMetaSeed.First();
            BlobMetaSeed.ElementAt(1).Container = ContainerMetaSeed.First();
        }

        public static List<BlobMetaTest> BlobMetaSeed = new List<BlobMetaTest>();

        public static List<ContainerMetaTest> ContainerMetaSeed = new List<ContainerMetaTest>();

        public static byte[] FileSeed = { 1, 1, 1, 1 };
        public static string Sotragesubject = "TestSubject";
    }
}
