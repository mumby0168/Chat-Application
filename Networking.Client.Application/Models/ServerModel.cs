using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Networking.Client.Application.Annotations;

namespace Networking.Client.Application.Models
{

    public enum ServerStatus
    {
        Connected,
        Disconnected,        
        Failed
    }

    /// <summary>
    /// A model to encapsulate the data needed to connect to the server.
    /// </summary>
    public class ServerModel : INotifyPropertyChanged
    {
        private string _ipAddress;

        public string IpAddress
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value; 
                OnPropertyChanged();
            }
        }
            
        private int _port;

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        private ServerStatus _serverStatus;

        public ServerStatus ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
       {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
