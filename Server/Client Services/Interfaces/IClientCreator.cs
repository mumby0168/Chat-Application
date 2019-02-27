using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server.Client_Services.Interfaces
{
    public interface IClientCreator
    {
         Task<bool> TryAddUser(TcpClient newClient);
    }
}