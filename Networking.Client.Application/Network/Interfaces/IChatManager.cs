using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Models;
using Sockets.DataStructures.Messages;
using User.System.Core.Model;

namespace Networking.Client.Application.Network.Interfaces
{
    public interface IChatManager
    {
        Dictionary<SocketUser, List<ChatMessageModel>> Chats { get; set; }

        void NewMessageCallback(Action<SocketUser> callbackFunc);

        Task SendChatMessage(ChatMessage chatMessage);
    }
}
