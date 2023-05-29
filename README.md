# Client-server

In this example, we have a simple client-server application. The server listens for incoming connections on a specified IP address and port. When a client connects, the server spawns a new thread to handle the client's requests. The client connects to the server using the server's IP address and port.

The server's Main method starts the server by creating a TcpListener and accepting client connections in a loop. Each client connection is handled in a separate thread using Task.Run.

The HandleClient method reads the client's request, processes it (in this case, simply appending it to a response string), and sends the response back to the client.

The client's Main method establishes a connection with the server, reads user input as the request, sends it to the server, and displays the server's response.

You can run the server and client applications in separate console windows to simulate the client-server communication.
