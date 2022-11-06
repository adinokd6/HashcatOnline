using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using WebHash.CustomExceptions;
using WebHash.Interfaces;
using WebHash.Services;

namespace WebHash_Tests
{
    public class CsvServiceTests
    {
        [Test]
        public void ImportCsvFile_ReturnProperListOfHashes()
        {
            //Arrange
            int csvHashesLine = 3;
            var properCsvFilePath = Path.Combine(Environment.CurrentDirectory, "csv-test-good.csv");
            var loggerMock = new Mock<ILoggerService>();
            var service = new CsvService(loggerMock.Object);

            //Act
            var results = service.ImportCsvFile(properCsvFilePath);

            //Assert
            Assert.IsNotEmpty(results);
            Assert.AreEqual(csvHashesLine, results.Count());
        }

        [Test]
        public void ImportCsvFile_ReturnNullOnEmptyCsv()
        {
            //Arrange
            var properCsvFilePath = Path.Combine(Environment.CurrentDirectory, "csv-test-bad-empty.csv");
            var loggerMock = new Mock<ILoggerService>();
            var service = new CsvService(loggerMock.Object);

            //Act
            var results = service.ImportCsvFile(properCsvFilePath);

            //Assert
            Assert.IsNull(results);
        }

        [Test]
        public void ImportCsvFile_ThrowExceptionOnCsvProblem()
        {
            //Arrange
            var properCsvFilePath = Path.Combine(Environment.CurrentDirectory, "csv-test-bad-badformat.csv");
            var loggerMock = new Mock<ILoggerService>();
            var service = new CsvService(loggerMock.Object);

            //Assert and Act
            Assert.Throws<CsvErrorException>(() => service.ImportCsvFile(properCsvFilePath));
        }


    }
}
