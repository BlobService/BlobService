using BlobService.Core.Tests.Entities;
using System.Collections.Generic;
using System.Linq;

namespace BlobService.Core.Tests
{
    internal static class TestData
    {
        static TestData()
        {
            BlobSeed.Add(new BlobTest()
            {
                Id = "1",
                ContainerId = "1",
                MimeType = "application/pdf",
                StorageSubject = Sotragesubject,
                OrigFileName = "test.pdf",
                SizeInBytes = 500
            });

            BlobSeed.Add(new BlobTest()
            {
                Id = "2",
                ContainerId = "1",
                MimeType = "application/zip",
                StorageSubject = "SS_2",
                OrigFileName = "test.zip",
                SizeInBytes = 400
            });

            ContainerSeed.Add(new ContainerTest()
            {
                Id = "1",
                Name = "Test"
            });

            ContainerSeed.First().Blobs.Add(BlobSeed.ElementAt(0));
            ContainerSeed.First().Blobs.Add(BlobSeed.ElementAt(1));

            BlobSeed.ElementAt(0).Container = ContainerSeed.First();
            BlobSeed.ElementAt(1).Container = ContainerSeed.First();
        }

        public static List<BlobTest> BlobSeed = new List<BlobTest>();
        public static List<BlobMetaDataTest> BlobMetaDataSeed = new List<BlobMetaDataTest>();
        public static List<ContainerTest> ContainerSeed = new List<ContainerTest>();

        public static byte[] FileSeed = { 1, 1, 1, 1 };
        public static string Sotragesubject = "TestSubject";
    }
}
