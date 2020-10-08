using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using MarsRoverPhotosAPI.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace MarsRoverPhotosAPITests
{
    [TestClass]
    public class MarsRoverPhotosControllerTests
    {
        [TestMethod]
        public async Task GetPhotosAsync_Date_IsNull()
        {
            //Arrange
            string date = null;
            var mockAPIConfig = new Mock<IConfiguration>();
            mockAPIConfig.SetupGet(x => x[It.Is<string>(s => s == "DatesFilePath" || s == "DownloadPhotosDirectoryPath" || s == "MarsRoverPhotosQueryByEarthDateAPI_URL")]).Returns("mockvalue");

            var mockAPIEnv = new Mock<IWebHostEnvironment>();
            mockAPIEnv.SetupGet(x => x.WebRootPath).Returns("mockvalue");

            var controller = new MarsRoverPhotosController(mockAPIConfig.Object, mockAPIEnv.Object);

            //Act
            var result = await controller.GetPhotosAsync(date);

            //Arrest
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public async Task GetPhotosAsync_InValidDate()
        {
            //Arrange
            string date = "mockvalue";
            var mockAPIConfig = new Mock<IConfiguration>();
            mockAPIConfig.SetupGet(x => x[It.Is<string>(s => s == "DatesFilePath" || s == "DownloadPhotosDirectoryPath" || s == "MarsRoverPhotosQueryByEarthDateAPI_URL")]).Returns("mockvalue");

            var mockAPIEnv = new Mock<IWebHostEnvironment>();
            mockAPIEnv.SetupGet(x => x.WebRootPath).Returns("mockvalue");

            var controller = new MarsRoverPhotosController(mockAPIConfig.Object, mockAPIEnv.Object);

            //Act
            var result = await controller.GetPhotosAsync(date);

            //Arrest
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
