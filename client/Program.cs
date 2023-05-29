// Client Program
using System;
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
        NetworkStream stream = _tcpClient.GetStream();

        byte[] requestData = Encoding.ASCII.GetBytes(request);
        stream.Write(requestData, 0, requestData.Length);

        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        return response;
    }

    public static void Main()
    {
        string serverIP = "localhost";
        int serverPort = 8888;

        ITcpClient tcpClient = new TcpClientWrapper();
        Client client = new Client(tcpClient, serverIP, serverPort);

        while(true)
        {
            Console.WriteLine("Enter your request:");
            string? request = Console.ReadLine();
            if (request == null) break;
            string response = client.SendMessageToServer(request);

            Console.WriteLine(response);
        }
    }
}

public interface ITcpClient
{
    void Connect(string serverIP, int serverPort);
    NetworkStream GetStream();
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

    public NetworkStream GetStream()
    {
        return _tcpClient.GetStream();
    }
}
