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
            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            // Handle client connection in a separate thread
            System.Threading.Tasks.Task.Run(() => HandleClient(client));
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received request from client: " + request);

            string response = _requestProcessor.ProcessRequest(request);

            byte[] responseData = Encoding.ASCII.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Sent response to client: " + response);
        }
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
    TcpClient AcceptTcpClient();
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

    public TcpClient AcceptTcpClient()
    {
        return _tcpListener.AcceptTcpClient();
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
