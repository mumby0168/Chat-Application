using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server.Client_Services.Interfaces
{
    /// <summary>
    /// Responsible for adding users to the chat session. 
    /// </summary>
    public interface IClientCreator
    {
        /// <summary>
        /// Tries to add a user to the user collection.
        /// </summary>
        /// <param name="newClient"></param>
        /// <returns></returns>
         Task<bool> TryAddUser(TcpClient newClient);
    }
}