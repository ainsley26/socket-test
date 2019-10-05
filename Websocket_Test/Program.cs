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
            Console.WriteLine("Starting async test...");
            Console.WriteLine(" > Enter URL to connect to: ");
            string uri = Console.ReadLine();
            Connect(uri).Wait();
            Console.WriteLine("Enter something to send...");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static async Task Connect(string uri)
        {
            // Declare the websocket object
            ClientWebSocket webSocket = null;

            // Try and connect
            try
            {
                // Create the websocket object
                webSocket = new ClientWebSocket();

                // Wait for a connection to be made
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                await Task.WhenAll(Receive(webSocket), Send(webSocket));
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                // Finish up by closing the websocket if it exists
                if (webSocket != null) webSocket.Dispose();

                Console.WriteLine("\nClientWebSocket has been closed.");
            }
        }

        private static async Task Send(ClientWebSocket webSocket)
        {
            // While the websocket connection is open
            while (webSocket.State == WebSocketState.Open)
            {
                // Get the message to send
                string msg = Console.ReadLine();
                byte[] buffer = Encoding.GetBytes(msg);

                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);
                Console.WriteLine("Sent: " + msg);

                await Task.Delay(1000);
            }
        }

        private static async Task Receive(ClientWebSocket webSocket)
        {
            // Create a buffer for input 
            byte[] buffer = new byte[1024];

            // While the websocket connection is open
            while (webSocket.State == WebSocketState.Open)
            {
                // Grab the message
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    Console.WriteLine("Receive: " + Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
                }
            }
        }
    }
}
