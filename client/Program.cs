// Client Program
using System;
using System.Net.Sockets;
using System.Text;

class Client
{
    static void Main()
    {
        string serverIP = "localhost";
        int serverPort = 8888;

        TcpClient client = new TcpClient();
        client.Connect(serverIP, serverPort);

        Console.WriteLine("Connected to server!");

        NetworkStream stream = client.GetStream();

        while (true)
        {
            Console.WriteLine("Enter a request to send to the server:");
            string? request = Console.ReadLine();

            if (request == null)
            {
                continue;
            }

            byte[] requestData = Encoding.ASCII.GetBytes(request);
            stream.Write(requestData, 0, requestData.Length);

            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            Console.WriteLine("Server response: " + response);
        }
    }
}
