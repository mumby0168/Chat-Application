namespace Server.Client_Services.Interfaces
{
    public interface IClientDisconnector
    {
        void UserDisconnected(ushort userId);    
    }
}