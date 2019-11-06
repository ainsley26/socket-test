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
            // Create the websocket handler
            Console.WriteLine("Starting websocket test.");
            SocketHandler handler = new SocketHandler();
            //SocketHandler.Connect("ws://192.168.178.18:9293").Wait();
            SocketHandler.Connect("ws://127.0.0.1:9293").Wait();
        }
    }
}
