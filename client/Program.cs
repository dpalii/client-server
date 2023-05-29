// Client Program
using System;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

public class Client
{
    private readonly ITcpClient _tcpClient;

    public Client(ITcpClient tcpClient, string serverIP, int serverPort)
    {
        _tcpClient = tcpClient;
        _tcpClient.Connect(serverIP, serverPort);
        Console.WriteLine("Connected to server!");
    }

    public string SendMessageToServer(string request)
    {
        INetworkStream stream = _tcpClient.GetStream();

        byte[] requestData = Encoding.ASCII.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);

        
        string response = stream.Read();

        return response;
    }

    public static void Main()
    {
        string serverIP = "localhost";
        int serverPort = 8888;

        while(true)
        {
            Console.WriteLine("Enter your request:");
            string? request = Console.ReadLine();
            ITcpClient tcpClient = new TcpClientWrapper();
            Client client = new Client(tcpClient, serverIP, serverPort);
            if (request == null) break;
            string response = client.SendMessageToServer(request);

            Console.WriteLine(response);
        }
    }
}

public interface ITcpClient
{
    void Connect(string serverIP, int serverPort);
    INetworkStream GetStream();
}

public interface INetworkStream
{
    public void Write(byte[] buffer, int offset, int count);
    public string Read();
}

public class NetworkStreamWrapper : INetworkStream
{
    private readonly NetworkStream _networkStream;

    public NetworkStreamWrapper(TcpClient tcpClient)
    {
        _networkStream = tcpClient.GetStream();
    }

    public string Read()
    {
        byte[] buffer = new byte[1024];
        int bytesRead = _networkStream.Read(buffer, 0, buffer.Length);

        return Encoding.ASCII.GetString(buffer, 0, bytesRead);
    }

    public void Write(byte[] buffer, int offset, int count)
    {
        _networkStream.Write(buffer, offset, count);
    }
}

public class TcpClientWrapper : ITcpClient
{
    private readonly TcpClient _tcpClient;

    public TcpClientWrapper()
    {
        _tcpClient = new TcpClient();
    }

    public void Connect(string serverIP, int serverPort)
    {
        _tcpClient.Connect(serverIP, serverPort);
    }

    public INetworkStream GetStream()
    {
        return new NetworkStreamWrapper(_tcpClient);
    }
}
