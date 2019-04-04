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

        private List<Action<int>> _callbacks;

        public ChatManager(INetworkConnectionController networkConnectionController)
        {
            _networkConnectionController = networkConnectionController;
            _networkConnectionController.MessageReceivedEventHandler += MessageReceived;
            Chats = new Dictionary<int, List<object>>();
            _callbacks = new List<Action<int>>();
        }

        public Dictionary<int, List<object>> Chats { get; set; }

        public void NewMessageCallback(Action<int>callbackFunc)
        {
            _callbacks.Add(callbackFunc);
        }

        public async Task SendChatMessage(ChatMessage chatMessage)
        {
            if (Chats.TryGetValue(chatMessage.UserToId, out var chat))
            {
                chat.Add(new ChatMessageModel() { IsSent = true, Message = chatMessage.Message, TimeStamp = DateTime.Now });

                CallCallbacks(chatMessage.UserToId);

                await _networkConnectionController.SendMessage(chatMessage);
            }               
        }

        public async Task SendImageMessage(ImageMessage imageMessage)
        {
            if (Chats.TryGetValue(imageMessage.UserToId, out var chat))
            {
                chat.Add(new ImageMessageModel
                {
                    ImageData = imageMessage.ImageData,
                    TimeStamp = DateTime.Now,
                    IsSent = true
                });

                CallCallbacks(imageMessage.UserToId);

                await _networkConnectionController.SendMessage(imageMessage);
            }
        }

        private void MessageReceived(object sender, MessageReceivedEventArgs args)
        {                    
            switch (args.Message.MessageType)
            {                
                case MessageType.Chat:
                    ProcessChatMessage(args.Message as ChatMessage);
                    break;
                case MessageType.UserOffline:
                    ProcessUserOffline((UserOfflineMessage)args.Message);
                    break;
                case MessageType.Image:
                    ProcessImageMessage((ImageMessage)args.Message);
                    break;

            }
        }

        private void ProcessUserOffline(UserOfflineMessage message)
        {
            if (Chats.ContainsKey(message.UsersId))
                Chats.Remove(message.UsersId);


            if(Chats.Count > 1)
                CallCallbacks(Chats.First().Key);
        }

        private void ProcessImageMessage(ImageMessage imageMessage)
        {
            Debug.WriteLine("image message sent.");
            if (Chats.TryGetValue(imageMessage.UserFromId, out var chat))
            {
                chat.Add(new ImageMessageModel()
                {
                    ImageData = imageMessage.ImageData,
                    IsSent = false,
                    TimeStamp = DateTime.Now
                });

                CallCallbacks(imageMessage.UserFromId);
            }
        }

        private void ProcessChatMessage(ChatMessage chatMessage)
        {
            if (Chats.TryGetValue(chatMessage.UserFromId, out var chat))
            {
                chat.Add(new ChatMessageModel()
                {
                    IsSent = false,
                    Message = chatMessage.Message,
                    TimeStamp = DateTime.Now
                });

                CallCallbacks(chatMessage.UserFromId);
            }            
        }        

        private void CallCallbacks(int userId)
        {
            foreach (var callback in _callbacks)
            {
                callback.Invoke(userId);
            }
        }
    }
}
