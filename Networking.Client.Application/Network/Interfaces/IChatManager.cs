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
    /// <summary>
    /// A manager to handle all of the interactions with chats from the sever.
    /// </summary>
    public interface IChatManager
    {
        Dictionary<int, List<object>> Chats { get; set; }

        void NewMessageCallback(Action<int> callbackFunc);

        Task SendChatMessage(ChatMessage chatMessage);

        Task SendImageMessage(ImageMessage imageMessage);
    }
}
