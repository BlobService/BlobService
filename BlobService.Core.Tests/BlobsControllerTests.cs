using BlobService.Core.Controllers;
using BlobService.Core.Models;
using BlobService.Core.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;

namespace BlobService.Core.Tests
{
    public class BlobsControllerTests
    {
        private BlobsController _controller;
        public BlobsControllerTests()
        {
            _controller = new BlobsController(
                new BlobServiceOptions(),
                new LoggerFactoryMock(),
                new StorageServiceMock(),
                new BlobMetaStoreMock(),
                new ContainerMetaStoreMock());
        }

        [Fact]
        public async void GetBlobByIdAsync_ReturnsNotFound()
        {
            var result = await _controller.GetBlobByIdAsync("not_existing_id");
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void GetBlobByIdAsync_ReturnsOk()
        {
            var result = await _controller.GetBlobByIdAsync(TestData.BlobMetaSeed.First().Id);
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void DownloadBlobAsync_ReturnsNotFound()
        {
            var result = await _controller.RawBlobAsync("not_existing_id");
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void DownloadBlobAsync_Ok()
        {
            //var result = await _controller.DownloadBlobAsync(TestData.BlobMetaSeed.First().Id);
            //var byteArray = await result.Content.ReadAsByteArrayAsync();
            //Assert.True(StructuralComparisons.StructuralEqualityComparer.Equals(TestData.FileSeed, byteArray));
        }

        [Fact]
        public async void AddBlobAsync_ReturnsBadRequest()
        {
            var result = await _controller.AddBlobAsync(TestData.ContainerMetaSeed.First().Id, null);
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async void AddBlobAsync_ReturnsNotFound()
        {
            var result = await _controller.AddBlobAsync("not_existing_id", new FileMock());
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void AddBlobAsync_ReturnsBadRequestLimitExceed()
        {
            var result = await _controller.AddBlobAsync(TestData.ContainerMetaSeed.First().Id, new FileMock((Constants.DefaultBlobSizeLimitInMB + 2 ) * 1024 * 1024 ));
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async void AddBlobAsync_ReturnsOk()
        {
            var result = await _controller.AddBlobAsync(TestData.ContainerMetaSeed.First().Id, new FileMock());
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void AddBlobAsync_ReturnsCorrectData()
        {
            var result = await _controller.AddBlobAsync(TestData.ContainerMetaSeed.First().Id, new FileMock()) as OkObjectResult;

            if (result?.Value is BlobViewModel blob)
            {
                Assert.Equal(blob.ContainerId, TestData.ContainerMetaSeed.First().Id);
                Assert.Equal(blob.MimeType, "text/plain");
                Assert.Equal(blob.SizeInBytes, TestData.FileSeed.Length);
                Assert.Equal(blob.OrigFileName, "Test.txt");
                Assert.Equal(blob.DownloadRelativeUrl, $"/blobs/{blob.Id}/raw");
            }
            else
            {
                Assert.True(false, "Blob can't be null");
            }
        }

        [Fact]
        public async void UpdateBlobAsync_ReturnsBadRequest()
        {
            var result = await _controller.UpdateBlobAsync(TestData.BlobMetaSeed.First().Id, null);
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async void UpdateBlobAsync_ReturnsNotFound()
        {
            var result = await _controller.UpdateBlobAsync("not_existing_id", new FileMock());
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void UpdateBlobAsync_ReturnsBadRequestLimitExceed()
        {
            var result = await _controller.UpdateBlobAsync(TestData.BlobMetaSeed.First().Id, new FileMock((Constants.DefaultBlobSizeLimitInMB + 2) * 1024 * 1024));
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async void UpdateBlobAsync_ReturnsOk()
        {
            var result = await _controller.UpdateBlobAsync(TestData.BlobMetaSeed.First().Id, new FileMock());
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void DeleteBlobAsync_NotFound()
        {
            var result = await _controller.DeleteBlobAsync("not_existing_id");
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void DeleteBlobAsync_Ok()
        {
            var result = await _controller.DeleteBlobAsync(TestData.BlobMetaSeed.First().Id);
            Assert.IsType(typeof(OkResult), result);
        }
    }
}
