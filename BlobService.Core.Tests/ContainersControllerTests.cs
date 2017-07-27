using BlobService.Core.Controllers;
using BlobService.Core.Models;
using BlobService.Core.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlobService.Core.Tests
{
    public class ContainersControllerTests
    {
        private ContainersController _controller;
        public ContainersControllerTests()
        {
            _controller = new ContainersController(
                new BlobServiceOptions(),
                new LoggerFactoryMock(),
                new ContainerMetaStoreMock());
        }

        [Fact]
        public async void GetAllContainersAsync_ReturnsOk()
        {
            var result = await _controller.GetAllContainersAsync();
            Assert.True(result.Count() == TestData.ContainerMetaSeed.Count);
        }

        [Fact]
        public async void GetContainerByIdAsync_ReturnsOkAsync()
        {
            var result = await _controller.GetContainerByIdAsync(TestData.ContainerMetaSeed.FirstOrDefault().Id);
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void GetContainerByIdAsync_ReturnsNotFound()
        {
            var result = await _controller.GetContainerByIdAsync("not_existing_id");
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void GetContainerByNameAsync_ReturnsOk()
        {
            //var result = await _controller.GetContainerByNameAsync(TestData.ContainerMetaSeed.FirstOrDefault().Name);
            //Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void GetContainerByNameAsync_ReturnsNotFound()
        {
            //var result = await _controller.GetContainerByNameAsync("not_existing_name");
            //Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void AddContainerAsync_BadRequest()
        {
            var result = await _controller.AddContainerAsync(null);
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async Task AddContainerAsync_OkAsync()
        {
            var result = await _controller.AddContainerAsync(new ContainerCreateModel() { Name = "test" });
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async void UpdateContainerAsync_BadRequest()
        {
            var result = await _controller.UpdateContainerAsync(TestData.ContainerMetaSeed.FirstOrDefault().Id, null);
            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public async void UpdateContainerAsync_NotFound()
        {
            var result = await _controller.UpdateContainerAsync("not_existing_container_id", new ContainerCreateModel() { Name = "test" });
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async Task UpdateContainerAsync_OkAsync()
        {
            var result = await _controller.UpdateContainerAsync(TestData.ContainerMetaSeed.FirstOrDefault().Id, new ContainerCreateModel() { Name = "test" });
            Assert.IsType(typeof(OkObjectResult), result);
        }

        [Fact]
        public async Task DeleteContainerAsync_OkAsync()
        {
            var result = await _controller.DeleteContainerAsync(TestData.ContainerMetaSeed.FirstOrDefault().Id);
            Assert.IsType(typeof(OkResult), result);
        }

        [Fact]
        public async void ListBlobsAsync_NotFound()
        {
            var result = await _controller.ListBlobsAsync("not_existing_id");
            Assert.IsType(typeof(NotFoundResult), result);
        }

        [Fact]
        public async void ListBlobsAsync_Ok()
        {
            var result = await _controller.ListBlobsAsync(TestData.ContainerMetaSeed.FirstOrDefault().Id);
            Assert.IsType(typeof(OkObjectResult), result);
        }
    }
}
