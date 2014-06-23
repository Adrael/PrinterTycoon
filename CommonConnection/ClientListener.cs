
namespace CommonConnection
{
    public interface IClientListener
    {
        void ProcessDataFromServer(byte[] responseFromServer, int dataSize);
    }
}
