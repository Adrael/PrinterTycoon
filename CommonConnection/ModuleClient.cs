using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CommonConnection
{
    public class ModuleClient
    {
        private TcpClient _clientSocket;
        private readonly IClientListener _clientListener;

        public ModuleClient(IClientListener clientListener)
        {
            _clientListener = clientListener;
        }

        public bool TryConnectWith(IPAddress serverAddress, int serverPort)
        {
            _clientSocket = new TcpClient();
            IAsyncResult ar = _clientSocket.BeginConnect(serverAddress, serverPort, null, null);
            System.Threading.WaitHandle wh = ar.AsyncWaitHandle;
            try
            {
                if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2), false))
                {
                    _clientSocket.Close();
                    return false;
                }

                _clientSocket.EndConnect(ar);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR CONNECTING " + e.Message);
                return false;
            }
            finally
            {
                wh.Close();
            }
        }

        public void SendDataToServer(byte[] dataToSend)
        {
            if (!_clientSocket.Connected)
            {
                byte[] message = Encoding.ASCII.GetBytes("Aucune connexion");
                _clientListener.ProcessDataFromServer(message, message.Length);
            }

            NetworkStream streamToServer = _clientSocket.GetStream();
            streamToServer.Write(dataToSend, 0, dataToSend.Length);
            streamToServer.Flush();

            byte[] responseFromServer = new byte[2048];
            int bufferSize = (int) _clientSocket.ReceiveBufferSize;
            streamToServer.Read(responseFromServer, 0, 16);

            _clientListener.ProcessDataFromServer(responseFromServer, bufferSize);
        }

        public void StopConnection()
        {
            SendDataToServer(Encoding.ASCII.GetBytes("over"));
            _clientSocket.Close();
        }
    }
}
