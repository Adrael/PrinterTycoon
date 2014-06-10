
using System;

namespace CommonConnection
{
    public interface IServerListener
    {
        byte[] ProcessDataFromClient(byte[] dataFromClient, int dataSize);
        void MessageForListener(String dataFromClient);
    }
}
