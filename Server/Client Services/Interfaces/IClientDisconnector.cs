namespace Server.Client_Services.Interfaces
{
    /// <summary>
    /// A service to publish the user disconnected message to clients.
    /// </summary>
    public interface IClientDisconnector
    {

        void UserDisconnected(ushort userId);    
    }
}