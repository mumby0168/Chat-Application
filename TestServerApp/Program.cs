using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Sockets.DataStructures;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services;

namespace TestServerApp
{
    class Program
    {

        private static NetworkDataService service = new NetworkDataService();

        static async Task Main(string[] args)
        {

            Thread.Sleep(5000);
            Console.WriteLine("Enter an ip address");
            string ip = "";
            var client1Id = await SetupClient(ip);

            int i = 0;            

            do
            {
                Console.WriteLine("Enter a client id to send a message to: ");
                int id = int.Parse(Console.ReadLine());

                MessageBase msg = null;

                Console.WriteLine("enter a message type to send \n 1: chat \n 2: image");
                int answer = int.Parse(Console.ReadLine());

                switch (answer)
                {
                    case 1:
                        msg = new ChatMessage()
                        {
                            UserToId = (ushort)id,
                            Message = "test chat message",
                            UserFromId = 7
                        };
                        break;
                    case 2:
                        msg = new ImageMessage()
                        {
                            UserToId = (ushort) id,
                            ImageData = await File.ReadAllBytesAsync(
                                @"C:\Users\billy\Pictures\Game Art\mouse-spritesheet.png"),
                            UserFromId = 7
                        };
                        break;
                }


                await service.WriteAndEncodeMessageWithHeader(msg, client1Id.Item2);

                Console.ReadLine();
                i++;
            } while (i < 20);

            Console.ReadLine();
        }

        private static async Task<Tuple<int, NetworkStream>> SetupClient(string ip)
        {
            var client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse("172.30.142.49"), 2500));

            var random = new Random();

            var stream = client.GetStream();

            var msg = new ConnectRequestMessage()
            {
                UserId = 7,
                ChatEnterMessage = "Hello"
            };

            await service.WriteAndEncodeMessageWithHeader(msg, stream);

            BeginListening(stream);

            return new Tuple<int, NetworkStream>(msg.UserId, stream);
        }


        public static void BeginListening(NetworkStream stream)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (stream.DataAvailable)
                    {
                        var header = service.ReadAndDecodeHeader(stream).Result;

                        var msg = service.ReadAndDecodeMessage(header, stream).Result;

                        if (msg is NewUserOnlineMessage)
                        {
                            var newMsg = msg as NewUserOnlineMessage;
                            Console.WriteLine(newMsg.UserId + " Is Online.");
                        }
                        else if (msg is ChatMessage)
                        {
                            var newmsg = msg as ChatMessage;
                            Console.WriteLine($"Message from {newmsg.UserFromId} saying: {newmsg.Message}");
                        }
                        else
                        {
                            Console.WriteLine("Final:");
                            Console.WriteLine(msg.MessageType);
                        }
                    }
                }
            });
        }

    }
}