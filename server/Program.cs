// Server Program
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Server
{
    static void Main()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        int port = 8888;

        TcpListener listener = new TcpListener(ipAddress, port);
        listener.Start();
        Console.WriteLine("Server started and listening for incoming connections...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected!");

            // Handle client connection in a separate thread
            System.Threading.Tasks.Task.Run(() => HandleClient(client));
        }
    }

    static void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received request from client: " + request);

            string response = ProcessRequest(request);

            byte[] responseData = Encoding.ASCII.GetBytes(response);
            stream.Write(responseData, 0, responseData.Length);
            Console.WriteLine("Sent response to client: " + response);
        }
    }

    static string ProcessRequest(string request)
    {
        // Process the request and return a response
        return "Hello, client! Your request was: " + request;
    }
}
