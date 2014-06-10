
namespace CommonConnection
{
    public interface IClientListener
    {
        byte[] ProcessDataFromServer(byte[] responseFromServer, int dataSize);
    }
}
