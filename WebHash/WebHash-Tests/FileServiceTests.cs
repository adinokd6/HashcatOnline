using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using WebHash.DataModels;
using WebHash.Interfaces;
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
            var loggerMock = new Mock<ILoggerService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object, loggerMock.Object);

            //Act
            var result = service.GetFiles();
            
            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetFiles_ReturnListsWithElements()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;

            
            using (var context = new Context(options))
            {
                context.Files.Add(new File
                {
                    Name = "Name",
                    Hashes = new List<Hash>()
                    {
                        new Hash()
                        {
                            Name = "test",
                            OriginalString = "string"
                        }
                    }
                });
                context.SaveChanges();
            }
            var contextForTests = new Context(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Context)))
                .Returns(contextForTests);


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

            var loggerMock = new Mock<ILoggerService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object, loggerMock.Object);

            //Act
            var result = service.GetFiles();

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetFiles_ReturnEmptyListWhenFileExistsButHasnAnyHash()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;


            using (var context = new Context(options))
            {
                context.Files.Add(new File
                {
                    Name = "Name",
                });
                context.SaveChanges();
            }
            var contextForTests = new Context(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Context)))
                .Returns(contextForTests);


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

            var loggerMock = new Mock<ILoggerService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object, loggerMock.Object);

            //Act
            var result = service.GetFiles();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetHashesFromFile_ProperReturnElements()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;

            var fileId = System.Guid.NewGuid();
            using (var context = new Context(options))
            {
                context.Files.Add(new File
                {
                    Name = "Name",
                    Id = fileId,
                    Hashes = new List<Hash>()
                    {
                        new Hash()
                        {
                            Name = "test",
                            OriginalString = "string"
                        }
                    }
                });
                context.SaveChanges();
            }
            var contextForTests = new Context(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Context)))
                .Returns(contextForTests);


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
            var loggerMock = new Mock<ILoggerService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object, loggerMock.Object);

            //Act
            var result = service.GetHashesFromFile(fileId);

            //Assert
            Assert.IsNotEmpty(result);
        }

        [Test]
        public void GetHashesFromFile_NoFilesInDatabase()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Context>().UseInMemoryDatabase(databaseName: "MovieListDatabase").Options;

            var fileId = System.Guid.NewGuid();
            using (var context = new Context(options))
            {
                context.Files.Add(new File
                {
                    Name = "Name",
                    Id = fileId,
                    Hashes = new List<Hash>()
                });
                context.SaveChanges();
            }
            var contextForTests = new Context(options);

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(Context)))
                .Returns(contextForTests);


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
            var loggerMock = new Mock<ILoggerService>();
            var webHostMock = new Mock<IWebHostEnvironment>();

            var service = new FileService(csvMock.Object, serviceScopeFactory.Object, webHostMock.Object, loggerMock.Object);

            //Act
            var result = service.GetHashesFromFile(fileId);

            //Assert
            Assert.IsNull(result);
        }
    }
}