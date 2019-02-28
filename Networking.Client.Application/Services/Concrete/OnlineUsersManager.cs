using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.EventArgs;
using Networking.Client.Application.Models;
using Networking.Client.Application.Network.Interfaces;
using Sockets.DataStructures.Base;
using Sockets.DataStructures.Messages;
using User.System.Core;
using User.System.Core.Model;
using User.System.Core.Repository;

namespace Networking.Client.Application.Services.Concrete
{
    public class OnlineUsersManager : IOnlineUsersManager
    {
        private readonly IChatManager _chatManager;

        public OnlineUsersManager(INetworkConnectionController networkConnectionController, IChatManager chatManager)
        {
            _chatManager = chatManager;
            networkConnectionController.MessageReceivedEventHandler += MessageReceivedEventHandler;
            OnlineUsers = new ObservableCollection<SocketUser>();
        }

        private async void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e)
        {
            switch (e.Message.MessageType)
            {
                case MessageType.NewUserOnline:
                    await NewUserOnline(e.Message as NewUserOnlineMessage);
                    break;                    
            }
        }

        public ObservableCollection<SocketUser> OnlineUsers { get; }

        public void AddUser(SocketUser socketUser)
        {
            var user = OnlineUsers.FirstOrDefault(s => s.Id == socketUser.Id);
            if (user != null) OnlineUsers.Remove(user);           
        }

        private async Task NewUserOnline(NewUserOnlineMessage newUserOnlineMessage)
        {
            using (var uow = new UnitOfWork(new SocketDbContext()))
            {
                var user = await uow.SocketUserRepo.SingleOrDefaultAsync(u => u.Id == newUserOnlineMessage.UserId);
                if (user != null)
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        AddUser(user);                        
                    });

                    if (!_chatManager.Chats.ContainsKey(user.Id))
                    {
                        _chatManager.Chats.Add(user.Id, new List<ChatMessageModel>());
                    }
                }
            }
        }
    }
}
