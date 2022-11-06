

using Moq;
using NUnit.Framework;
using WebHash.CustomExceptions;
using WebHash.Interfaces;
using WebHash.Services;

namespace WebHash_Tests
{
    public class StartProgramServiceTests
    {
        [Test]
        public void StartDecryptionProcess_ReturnGoodDecryptedHash()
        {
            //Arrange
            string goodCmdInput = "-a 3 -m 100 fc19318dd13128ce14344d066510a982269c241b";
            string hashBeforeDecoding = "fc19318dd13128ce14344d066510a982269c241b";
            var desiredOutput = "good\r\n\r";

            var loggerMock = new Mock<ILoggerService>();

            var service = new StartProgramService(loggerMock.Object);

            //Act
            var result = service.StartDecryptionProcess(goodCmdInput, hashBeforeDecoding);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(desiredOutput, result.Item2);
        }

        [Test]
        public void StartDecryptionProcess_ThrowHashCatProblemExcepton()
        {
            //Arrange
            var loggerMock = new Mock<ILoggerService>();
            var service = new StartProgramService(loggerMock.Object, "bad-input", "bad-input", "bad-input", "bad-input");

            //Assert and act
            Assert.Throws<HashCatProblemException>(() => service.StartDecryptionProcess(null, null));
        }
    }
}
