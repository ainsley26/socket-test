using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Websocket_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connect("ws://192.168.178.17:9000").Wait();
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            Console.WriteLine("Starting websocket test.");

            // Create the websocket handler
            SocketHandler handler = new SocketHandler();
            SocketHandler.Connect("ws://192.168.178.18:9293").Wait();
        }

        //public static async Task Connect(string uri)
        //{
        //    // Declare the websocket object
        //    ClientWebSocket webSocket = null;

        //    // Try and connect
        //    try
        //    {
        //        // Create the websocket object
        //        webSocket = new ClientWebSocket();

        //        // Wait for a connection to be made
        //        await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
        //        await Task.WhenAll(Receive(webSocket), Send(webSocket));
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception: {0}", e);
        //    }
        //    finally
        //    {
        //        // Finish up by closing the websocket if it exists
        //        if (webSocket != null) webSocket.Dispose();

        //        Console.WriteLine("\nClientWebSocket has been closed.");
        //    }
        //}

        //private static async Task Send(ClientWebSocket webSocket)
        //{
        //    // While the websocket connection is open
        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        // Get the message to send
        //        Console.WriteLine("Enter a message to send: ");
        //        string msg = Console.ReadLine();
        //        byte[] buffer = Encoding.UTF8.GetBytes(msg);

        //        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        //        Console.WriteLine("Sent: " + msg);

        //        await Task.Delay(500);
        //    }
        //}

        //private static async Task Receive(ClientWebSocket webSocket)
        //{
        //    // Create a buffer for input 
        //    byte[] buffer = new byte[1024];

        //    // While the websocket connection is open
        //    while (webSocket.State == WebSocketState.Open)
        //    {
        //        // Clear the buffer
        //        for (int i = 0; i < 1024; ++i) buffer[i] = 0;

        //        // Grab the message
        //        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        if (result.MessageType == WebSocketMessageType.Close)
        //        {
        //            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        //        }
        //        else
        //        {
        //            Console.WriteLine("Receive: " + Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
        //        }
        //    }
        //}
    }
}
