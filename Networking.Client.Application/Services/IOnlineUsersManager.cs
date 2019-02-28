using System.Collections.Generic;
using System.Collections.ObjectModel;
using User.System.Core.Model;

namespace Networking.Client.Application.Services
{
    public interface IOnlineUsersManager
    {
        ObservableCollection<SocketUser> OnlineUsers { get; }

        void AddUser(SocketUser socketUser);
    }
}
