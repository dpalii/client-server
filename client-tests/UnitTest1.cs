using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace YourNamespace.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void SendMessageToServer_ValidRequest_ReturnsResponse()
        {
            // Arrange
            string serverIP = "127.0.0.1";
            int serverPort = 8888;
            string request = "Test request";
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);
            string expectedResponse = "Hello, client! Your request was: " + request;

            var tcpClientMock = new Mock<ITcpClient>();
            var networkStreamMock = new Mock<INetworkStream>();

            tcpClientMock.Setup(c => c.GetStream()).Returns(networkStreamMock.Object);
            networkStreamMock.Setup(s => s.Read()).Returns(expectedResponse);
            networkStreamMock.Setup(s => s.Write(It.Is<byte[]>(b => b.SequenceEqual(requestBytes)), 0, It.Is<int>(i => i == requestBytes.Length)));

            var client = new Client(tcpClientMock.Object, serverIP, serverPort);

            // Act
            string response = client.SendMessageToServer(request);

            // Assert
            Assert.AreEqual(expectedResponse, response);
        }
    }
}
