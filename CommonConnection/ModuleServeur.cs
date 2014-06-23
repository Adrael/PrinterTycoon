using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommonConnection
{
    public class ModuleServeur
    {
        private TcpListener _serverSocket;
        private IServerListener _moduleLister;
        private bool _running;
        private List<Thread> _listOfThread;
        private int _counter;
        private delegate void MessageSender(IServerListener listener, string message);

        public ModuleServeur(IServerListener listener)
        {
            _moduleLister = listener;
            _running = false;
            _counter = 0;
        }

        public void StartServer(IPAddress address, int port)
        {
            if (_serverSocket != null)
                _serverSocket.Stop();

            // _listOfThread = new List<Thread>();

            _serverSocket = new TcpListener(address, port);
            _serverSocket.Start();
            _moduleLister.MessageForListener("serveur démarré");
            
            // démarre le thread qui attend que les clients se connectent
            Thread thread = new Thread(WaitForAClient);
            thread.Name = "écoute les connection";
            thread.Start();
        }

        public void WaitForAClient()
        {
            try
            {
                TcpClient newClient = _serverSocket.AcceptTcpClient();
                _moduleLister.MessageForListener("nouveau client");

                new Thread(() => WaitForClientRequest(newClient)).Start();
            }
            catch(SocketException exception)
            {
                // le serveur arrête l'écoute
                _moduleLister.MessageForListener("le serveur n'accepte plus de nouvaux clients");
            }
            
        }

        private void WaitForClientRequest(TcpClient newClient)
        {
            NetworkStream stream = newClient.GetStream();
            byte[] messageFromClient = new byte[2048];
            _running = true;

            while (_running)
            {
                int byteRead = 0;
                try
                {
                    byteRead = stream.Read(messageFromClient, 0, 2048);

                    if (byteRead == 0)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    break;
                }

                ASCIIEncoding encoder = new ASCIIEncoding();
                Console.WriteLine("serveur recieves:\n\t" + encoder.GetString(messageFromClient, 0, byteRead) + "\n");

                try
                {
                    byte[] response = _moduleLister.ProcessDataFromClient(messageFromClient, byteRead);
                    stream.Write(response, 0, response.Length);
                    stream.Flush();
                }
                catch (Exception)
                {
                    Console.WriteLine("impossible d'envoyer les données au client");
                }
            }
            newClient.Close();
        }

        public void StopModule()
        {
            _running = false;
            _serverSocket.Stop();
        }

        public void SendMessageToListener(IServerListener listener, String message)
        {
            Console.WriteLine("envoie au listener : " + message);
            Console.WriteLine(listener.ToString());
            listener.MessageForListener(message);
        }

        public bool isRunning()
        {
            return _running;
        }
    }
}
