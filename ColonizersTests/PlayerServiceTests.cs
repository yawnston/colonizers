using Desktop.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace ColonizersTests
{
    public class PlayerServiceTests
    {
        [Fact]
        public void ShouldCreateHumanPlayers()
        {
            var loggerMock = new Mock<ILogger<PlayerService>>();
            var pythonServiceMock = new Mock<IPythonExecutableService>();
            var service = new PlayerService(loggerMock.Object, pythonServiceMock.Object);

            var testData = Enumerable.Repeat("Human Player", 4).ToArray();
            service.InitPlayers(testData);
            Assert.Equal("Human Player", service.Players[0].Name);
        }
    }
}
