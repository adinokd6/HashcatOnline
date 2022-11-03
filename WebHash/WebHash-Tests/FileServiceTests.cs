using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using WebHash.DataModels;
using WebHash.IServices;
using WebHash.Services;

namespace WebHash_Tests
{
    public class FileServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetFiles_ReturnEmptyLists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;

            var context = new Context(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Context)))
                .Returns(context);


            var serviceScope = new Mock<IServiceScope>();
            serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

            var serviceScopeFactory = new Mock<IServiceScopeFactory>();
            serviceScopeFactory
                .Setup(x => x.CreateScope())
                .Returns(serviceScope.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IServiceScopeFactory)))
                .Returns(serviceScopeFactory.Object);

            var csvMock = new Mock<ICsvService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object);

            var result = service.GetFiles();
            Assert.IsEmpty(result);

        }

/*        [Test]
        public void GetFiles_ReturnListsWithElements()
        {
            Assert.Pass();
        }*/
    }
}