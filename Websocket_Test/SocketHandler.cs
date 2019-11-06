using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Websocket_Test
{
    public class SocketHandler
    {
        // The websocket object
        static ClientWebSocket webSocket;

        public SocketHandler()
        {
            webSocket = null;
        }

        public static async Task Connect(string uri)
        {
            // Try and connect
            try
            {
                // Create the websocket object
                webSocket = new ClientWebSocket();

                // Wait for a connection to be made
                await webSocket.ConnectAsync(new Uri(uri), CancellationToken.None);

                // Continue until the connection dies
                await Task.WhenAll(Recieve(), ManualSend());
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to connect\nException: {0}", e);
            }
            finally
            {
                // Finish up by closing the websocket if it exists
                if (webSocket != null) webSocket.Dispose();

                Console.WriteLine("ClientWebSocket has been closed.");
            }
        }

        private static async Task Heartbeat()
        {
            // Just check if the connection is still alive
            while (webSocket.State == WebSocketState.Open)
            {
                await Task.Delay(1000);     // Check again in 1 second
            }
        }

        public static async Task Send(string msg)
        {
            // Check if the websocket connection is still open
            if (webSocket.State == WebSocketState.Open)
            {
                // Prepare and send the message across
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine("Sent: " + msg);
            }
        }

        private static async Task ManualSend()
        {
            // While the websocket connection is open
            while (webSocket.State == WebSocketState.Open)
            {
                // Get the message to send
                Console.WriteLine("Enter a message to send: ");
                string msg = Console.ReadLine();
                //byte[] buffer = Encoding.UTF8.GetBytes(msg);

                //await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                //Console.WriteLine("Sent: " + msg);
                await Send(msg);
            }
        }

        private static async Task Recieve()
        {
            // Create the buffer for the message
            byte[] buffer = new byte[1024];

            // While the websocket connection is open
            while (webSocket.State == WebSocketState.Open)
            {
                // Clear the buffer
                for (int i = 0; i < 1024; ++i) buffer[i] = 0;

                // Get the message
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                }
                else
                {
                    string msg = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                    Console.WriteLine("Received: " + msg);

                    // Actually do something with the message here...
                    var json = JObject.Parse(msg);

                    // 1. Extract the message
                    string cmd = (string)json["message"];

                    // 2. Perform the command
                    // TODO: review how best to handle different commands,
                    //       either a switch like this, or a map of func. ptrs.?
                    string ret = "";
                    switch (cmd)
                    {
                        case "status":
                            ret = GetStatus();
                            break;
                    }

                    // 3. Compile the result
                    JObject retJson = JObject.FromObject(new
                    {
                        to = (string)json["from"],
                        from = "our ip address",
                        message = ret,
                        timestamp = DateTime.Now,
                        authorization = "an auth token"
                    });

                    // 4. Send the result
                    await Send(retJson.ToString());
                }
            }
        }

        private static string GetStatus()
        {
            return "Okay";
        }
    }
}

/**
    Processes a message received on a websocket.
    The message should be a JSON consisting of the following fields:
        to:         the ip address or hostname of the message recipient
        from:       the ip adress or hostname of the message sender
        message:    the message for the recipient
        timestamp:  the time that the message was sent
        auth:       an auth token of some kind
 */
