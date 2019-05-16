namespace Server.Client_Services.Interfaces
{
    /// <summary>
    /// A service to publish the user disconnected message to clients.
    /// </summary>
    public interface IClientDisconnector
    {
        /// <summary>
        /// This is executed when a user should be disconnected. 
        /// </summary>
        /// <param name="userId"></param>
        void UserDisconnected(ushort userId);    
    }
}