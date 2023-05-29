// Server Program
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class Server
{
    private readonly ITcpListener _listener;
    private readonly IRequestProcessor _requestProcessor;

    public Server(ITcpListener listener, IRequestProcessor requestProcessor)
    {
        _listener = listener;
        _requestProcessor = requestProcessor;
    }

    public void Start()
    {
        _listener.Start();
        Console.WriteLine("Server started and listening for incoming connections...");

        while (true)
        {
            ITcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            // Handle client connection in a separate thread
            System.Threading.Tasks.Task.Run(() => HandleClient(client));
        }
    }

    public void HandleClient(ITcpClient client)
    {
        INetworkStream stream = client.GetStream();

        string request = stream.Read();
        Console.WriteLine("Received request from client: " + request);

        string response = _requestProcessor.ProcessRequest(request);

        byte[] responseData = Encoding.ASCII.GetBytes(response);
        stream.Write(responseData, 0, responseData.Length);
        Console.WriteLine("Sent response to client: " + response);
    }

    public static void Main()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8888;
        ITcpListener listener = new TcpListenerWrapper(ipAddress, port);
        RequestProcessor requestProcessor = new RequestProcessor();
        Server server = new Server(listener, requestProcessor);
        server.Start();
    }
}

public interface ITcpListener
{
    void Start();
    ITcpClient AcceptTcpClient();
}

public interface IRequestProcessor
{
    string ProcessRequest(string request);
}

public class TcpListenerWrapper : ITcpListener
{
    private readonly TcpListener _tcpListener;

    public TcpListenerWrapper(IPAddress ipAddress, int port)
    {
        _tcpListener = new TcpListener(ipAddress, port);
    }

    public void Start()
    {
        _tcpListener.Start();
    }

    public ITcpClient AcceptTcpClient()
    {
        return new TcpClientWrapper(_tcpListener.AcceptTcpClient());
    }
}

public class RequestProcessor : IRequestProcessor
{
    public string ProcessRequest(string request)
    {
        // Process the request and return a response
        return "Hello, client! Your request was: " + request;
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

    public TcpClientWrapper(TcpClient client)
    {
        _tcpClient = client;
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
