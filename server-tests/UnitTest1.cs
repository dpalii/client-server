using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Sockets;
using System.Text;

namespace YourNamespace.Tests
{
    [TestClass]
    public class ServerTests
    {
        [TestMethod]
        public void HandleClient_ValidRequest_ProcessesRequestAndSendsResponse()
        {
            // Arrange
            string request = "Test request";
            string expectedResponse = "Hello, client! Your request was: " + request;

            var clientMock = new Mock<ITcpClient>();
            var streamMock = new Mock<INetworkStream>();
            var listenerMock = new Mock<ITcpListener>();
            var requestProcessorMock = new Mock<IRequestProcessor>();

            streamMock.Setup(s => s.Read()).Returns(request);
            clientMock.Setup(c => c.GetStream()).Returns(streamMock.Object);
            listenerMock.Setup(l => l.AcceptTcpClient()).Returns(clientMock.Object);
            requestProcessorMock.Setup(rp => rp.ProcessRequest(request)).Returns(expectedResponse);

            var server = new Server(listenerMock.Object, requestProcessorMock.Object);

            // Act
            server.HandleClient(clientMock.Object);

            // Assert
            streamMock.Verify(s => s.Read(), Times.Once);
            streamMock.Verify(s => s.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()), Times.Once);
        }
    }
}