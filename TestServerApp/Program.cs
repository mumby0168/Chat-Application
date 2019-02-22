using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Sockets.DataStructures;
using Sockets.DataStructures.Messages;
using Sockets.DataStructures.Services;

namespace TestServerApp
{
    class Program
    {

        private static NetworkDataService service = new NetworkDataService();

        static async Task Main(string[] args)
        {
            var client1Id = await SetupClient();

            Thread.Sleep(2000);

            var client2Id = await SetupClient();

            int i = 0;

            do
            {
                Console.WriteLine("Enter a client id to send a message to: ");
                int id = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter message to send: ");
                string message = Console.ReadLine();

                var chatmsg = new ChatMessage()
                {
                    UserToId = (ushort) id,
                    UserFromId = (ushort) client1Id.Item1,
                    Message = message
                };

                await service.WriteAndEncodeMessageWithHeader(chatmsg, client1Id.Item2);

                Console.ReadLine();
                i++;
            } while (i < 5);

            Console.ReadLine();
        }

        private static async Task<Tuple<int, NetworkStream>> SetupClient()
        {
            var client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Parse("172.20.10.10"), 2500));

            var random = new Random();

            var stream = client.GetStream();

            var msg = new ConnectRequestMessage()
            {
                UserId = (ushort)random.Next(1, 20),
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
                    }
                }
            });
        }

    }
}