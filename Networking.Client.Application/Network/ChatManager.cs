using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using User.System.Core.Model;

namespace Networking.Client.Application.Network
{
    public class ChatManager : IChatManager
    {
        private readonly INetworkConnectionController _networkConnectionController;

        private List<Action<SocketUser>> _callbacks;

        public ChatManager(INetworkConnectionController networkConnectionController)
        {
            _networkConnectionController = networkConnectionController;
            _networkConnectionController.MessageReceivedEventHandler += MessageReceived;
            Chats = new Dictionary<SocketUser, List<ChatMessageModel>>();
            _callbacks = new List<Action<SocketUser>>();
        }

        public Dictionary<SocketUser, List<ChatMessageModel>> Chats { get; set; }

        public void NewMessageCallback(Action<SocketUser>callbackFunc)
        {
            _callbacks.Add(callbackFunc);
        }

        public async Task SendChatMessage(ChatMessage chatMessage)
        {
            var userTo = Chats.FirstOrDefault(c => c.Key.Id == chatMessage.UserToId);

            userTo.Value.Add(new ChatMessageModel(){IsSent = true, Message = chatMessage.Message, TimeStamp = DateTime.Now});

            CallCallbacks(userTo.Key);

            await _networkConnectionController.SendMessage(chatMessage);            
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Debug.WriteLine("Chat manager has data.");

            Debug.WriteLine(args.Message.MessageType);

            switch (args.Message.MessageType)
            {                
                case MessageType.Chat:
                    ProcessChatMessage(args.Message as ChatMessage);
                    break;
            }
        }

        private void ProcessChatMessage(ChatMessage chatMessage)
        {
            var chat = Chats.FirstOrDefault(d => d.Key.Id == chatMessage.UserFromId);

            if (chat.Value == null)
            {
                Debug.WriteLine("list is null.");
                return;                
            }
            
            chat.Value.Add(new ChatMessageModel()
            {
                IsSent = false,
                Message = chatMessage.Message,
                TimeStamp = DateTime.Now
            });

            if (chat.Key == null) return;           
            CallCallbacks(chat.Key);
        }

        private void CallCallbacks(SocketUser socketUser)
        {
            foreach (var callback in _callbacks)
            {
                callback.Invoke(socketUser);
            }
        }
    }
}
