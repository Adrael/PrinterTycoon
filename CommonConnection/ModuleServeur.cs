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
        private List<Thread> _listOfClientsThread;
        private int _counter;
        private bool _waitingForClient;

        private delegate void MessageSender(IServerListener listener, string message);

        public ModuleServeur(IServerListener listener)
        {
            _moduleLister = listener;
            _listOfClientsThread = new List<Thread>();
            _waitingForClient = false;
            _running = false;
            _counter = 0;
        }

        public void StartServer(IPAddress address, int port)
        {
            if (_serverSocket != null)
                _serverSocket.Stop();
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
            _waitingForClient = true;

            try
            {
                TcpClient newClient = _serverSocket.AcceptTcpClient();
                _moduleLister.MessageForListener("nouveau client");

                Thread listenerClientRequests = new Thread(() => WaitForClientRequest(newClient));
                listenerClientRequests.Start();
                _listOfClientsThread.Add(listenerClientRequests);
            }
            catch(SocketException exception)
            {
                // le serveur arrête l'écoute
                _moduleLister.MessageForListener("le serveur n'accepte plus de nouvaux clients");
            }

            _waitingForClient = false;
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
            if (_waitingForClient)
            {
                _serverSocket.Stop();
            }
            _waitingForClient = false;
            _running = false;
            
            foreach (Thread thread in _listOfClientsThread)
            {
                thread.Abort();
            }
        }

        public void SendMessageToListener(IServerListener listener, String message)
        {
            Console.WriteLine("envoie au listener : " + message);
            Console.WriteLine(listener.ToString());
            listener.MessageForListener(message);
        }

        public bool IsRunning()
        {
            return _waitingForClient;
        }
    }
}
